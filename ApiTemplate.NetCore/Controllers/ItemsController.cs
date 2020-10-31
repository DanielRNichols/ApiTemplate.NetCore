using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTemplate.NetCore.Data;
using ApiTemplate.NetCore.DTOs;
using ApiTemplate.NetCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiTemplate.NetCore.Controllers
{
    /// <summary>
    /// Controller for interacting with Item endpoint
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public ItemsController(IItemRepository itemRepository,
                               ILoggerService logger,
                               IMapper mapper)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        // GET: api/<ItemsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                _logger.LogInfo("GetItems");
                var items = await _itemRepository.GetAll();
                var response = _mapper.Map<IList<ItemDTO>>(items);
                _logger.LogInfo("Successfully retrieved Items");

                return Ok(response);
            }
            catch (Exception e)
            {
                return ServerError(e);
            }
        }

        /// <summary>
        /// Get an item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                _logger.LogInfo($"GetItem by id: {id}");
                var item = await _itemRepository.GetById(id);
                if(item == null)
                {
                    _logger.LogWarn($"Item not found: {id}");

                    return NotFound();
                }
                var response = _mapper.Map<ItemDTO>(item);
                _logger.LogInfo($"Successfully retrieved Item {id}");

                return Ok(response);
            }
            catch (Exception e)
            {
                return ServerError(e);
            }
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns></returns>
        // POST api/<ItemsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ItemCreateDTO itemDTO)
        {
            try
            {
                _logger.LogInfo("Attempted to create Item");
                if (itemDTO == null)
                {
                    _logger.LogWarn($"Empty request submitted");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Invalid request submitted");
                    return BadRequest(ModelState);
                }
                var item = _mapper.Map<Item>(itemDTO);

                var isSuccess = await _itemRepository.Create(item);
                if (!isSuccess)
                {
                    return ServerError("Item creation failed");
                }
                _logger.LogInfo("Item created");
                return Created("", new { item });
            }
            catch (Exception e)
            {
                return ServerError(e);
            }
        }

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemDTO"></param>
        /// <returns></returns>
        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ItemCreateDTO itemDTO)
        {
            try
            {
                _logger.LogInfo("Attempted to update Item");
                if (id < 1)
                {
                    _logger.LogWarn($"Invalid id submitted: {id}");
                    return BadRequest();
                }

                if (itemDTO == null)
                {
                    _logger.LogWarn($"Empty request submitted");
                    return BadRequest();
                }
                var exists = await _itemRepository.Exists(id);
                if (!exists)
                {
                    _logger.LogWarn($"Item with id {id} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Invalid request submitted");
                    return BadRequest(ModelState);
                }

                var item = _mapper.Map<Item>(itemDTO);
                // force item.Id to be id passed in 
                item.Id = id;

                var isSuccess = await _itemRepository.Update(item);
                if (!isSuccess)
                {
                    return ServerError("Item update failed");
                }
                _logger.LogInfo("Item updated");
                return Ok(new { item });
            }
            catch (Exception e)
            {
                return ServerError(e);
            }
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo("Attempted to delete Item");
                if (id < 1)
                {
                    _logger.LogWarn($"Empty request submitted");
                    return BadRequest();
                }
                var item = await _itemRepository.GetById(id);
                if (item == null)
                {
                    _logger.LogWarn($"Item with id ${id} was not found");
                    return NotFound();
                }

                var isSuccess = await _itemRepository.Delete(item);
                if (!isSuccess)
                {
                    return ServerError("Item delete failed");
                 }
                _logger.LogInfo("Item deleted");
                return Ok(id);
            }
            catch (Exception e)
            {
                return ServerError(e);
            }
        }

        private ObjectResult ServerError(string msg)
        {
            _logger.LogServerError(msg);
            return StatusCode(500, "Server Error");
        }
        private ObjectResult ServerError(Exception e)
        {
            _logger.LogServerError(e);
            return StatusCode(500, "Server Error");
        }
    }
}

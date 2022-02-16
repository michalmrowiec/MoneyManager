using BlazorApp1.Server.Services;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorApp1.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tracker")]
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerService _trackerService;
        public TrackerController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        [HttpPost]
        public ActionResult CreateRecord([FromBody] RecordItemDto recordItem)
        {
            _trackerService.Post(recordItem);
            return Created("", null);
        }

        [HttpGet]
        public ActionResult<List<RecordItemDto>> GetAllRecords()
        {
            return Ok(_trackerService.GetAllRecords());
        }

        [HttpGet]
        [Route("cat/{categoryId}")]
        public ActionResult<List<RecordItemDto>> GetRecordsForCategory([FromRoute] int categoryId)
        {
            return Ok(_trackerService.GetRecordsForCategory(categoryId));
        }

        [HttpDelete("{recordId}")]
        public ActionResult DeleteRecord([FromRoute] int recordId)
        {
            _trackerService.Delete(recordId);
            return NoContent();
        }

        [HttpPatch]
        public ActionResult UpdateRecord([FromBody] RecordItemDto recordItem)
        {
            _trackerService.Update(recordItem);
            return Ok();
        }
    }
}

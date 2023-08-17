using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Attributes;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset;
using MoneyManager.Application.Functions.CryptoAssets.Commands.DeleteCryptoAsset;
using MoneyManager.Application.Functions.CryptoAssets.Commands.UpdateCryptoAsset;
using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Application.Functions.CryptoAssets.Queries.CryptocurrencySymbolsAndNames;
using MoneyManager.Application.Functions.CryptoAssets.Queries.GetAllCryptoAssets;

namespace MoneyManager.API.Controllers
{
    [ApiKeyRequired]
    [Authorize]
    [ApiController]
    [Route("api/crypto-assets")]
    public class CryptoAssetController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContextService _userContextService;
        public CryptoAssetController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecord([FromBody] CreateCryptoAssetCommand recordItem)
        {
            recordItem.UserId = _userContextService.GetUserId;
            var record = await _mediator.Send(recordItem);
            return Created("", record);
        }

        [HttpGet("symbols-and-names")]
        public async Task<ActionResult<Dictionary<string, string>>> GetCryptocurrencySymbolsAndNames()
        {
            return Ok(await _mediator.Send(new CryptocurrencySymbolsAndNamesQuery()));
        }

        [HttpGet]
        public async Task<ActionResult<List<CryptoAssetDto>>> GetAllRecords()
        {
            return Ok(await _mediator.Send(new GetAllCryptoAssetsQuery(_userContextService.GetUserId)));
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            await _mediator.Send(new DeleteCryptoAssetCommand(_userContextService.GetUserId, recordId));
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRecord([FromBody] UpdateCryptoAssetCommand recordItem)
        {
            recordItem.UserId = _userContextService.GetUserId;
            await _mediator.Send(recordItem);
            return Ok();
        }

    }
}

using MoneyManager.Client.Models.ViewModels.Interfaces;
using MoneyManager.Client.Services;

namespace MoneyManager.Client
{
    public class DeleteDialog<T> where T : IId
    {
        public  IHttpRecordService _httpRecordService;
        public ErrorMessage _errorMessage;
        private readonly string _uriDelete;
        public DeleteDialog(IHttpRecordService httpRecordService, string uriDelete, ErrorMessage errorMessage)
        {
            _httpRecordService = httpRecordService;
            _uriDelete = uriDelete;
            _errorMessage = errorMessage;
        }
        public Func<Task>? invokeOnClose;
        public bool DeleteDialogOpen { get; set; } = false;
        public T? RecordToDelete { get; set; }


        public async Task CloseDeleteDialog(bool deleteConfirmed)
        {
            if (deleteConfirmed && RecordToDelete != null)
            {
                await Delete(RecordToDelete.Id);
            }
            DeleteDialogOpen = false;

            if (invokeOnClose == null)
                return;

            var tasks = invokeOnClose.GetInvocationList().Select(handler => ((Func<Task>)handler).Invoke()).ToList();
            await Task.WhenAll(tasks);
        }

        public void OpenDeleteDialog(T record, Action? invokeOnClose)
        {
            DeleteDialogOpen = true;
            RecordToDelete = record;

            invokeOnClose?.Invoke();
        }

        public async Task Delete(int id)
        {
            try
            {
                var response = await _httpRecordService.DeleteItem(id, _uriDelete);
                if (response == null) return;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    _errorMessage.Message = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _errorMessage.Message = ex.Message;
            }
        }
    }
}

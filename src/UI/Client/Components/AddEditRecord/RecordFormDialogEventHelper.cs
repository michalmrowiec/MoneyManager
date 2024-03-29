﻿using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Models.ViewModels;

namespace MoneyManager.Client.Components.AddEditRecord
{
    public partial class RecordFormDialogEventHelper
    {
        private bool _addIsOpen;
        public bool AddIsOpen
        {
            get
            {
                return _addIsOpen;
            }
            set
            {
                _addIsOpen = value;
                FuncsOnOpen?.ForEach(async x => await x.InvokeAsync());
            }
        }

        public FormRecordModel? RecordToEdit { get; set; }
        public TypeOfAddEditFormDialog TypeOfDialog { get; set; } = TypeOfAddEditFormDialog.Add;
        public List<EventCallback>? FuncsOnOpen { get; set; } = new();
        public List<EventCallback>? FuncsOnClose { get; set; } = new();

        public void Execute()
        {
            AddIsOpen = false;
            RecordToEdit = null;
            FuncsOnClose?.ForEach(async x => await x.InvokeAsync());
            TypeOfDialog = TypeOfAddEditFormDialog.Add;
        }

        public enum TypeOfAddEditFormDialog
        {
            Add,
            EditRecord,
            EditRecurringRecord
        }
    }
}

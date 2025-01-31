﻿using eXtensionSharp;
using MudBlazor;
using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.DetailView;

public abstract class MudDetailViewComposite<TRetrieveItem> : MudViewCompositeBase, IMudDetailViewComposite<TRetrieveItem>
{
    protected MudDetailViewComposite(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }

    public TRetrieveItem RetrieveItem { get; set; }
    public Func<Task<Results>> OnSubmit { get; set; }
    public Func<Task<Results>> OnRetrieve { get; set; }

    public virtual async Task Retrieve()
    {
        if(OnRetrieve.xIsEmpty()) return;
        
        var dlg = await this.ShowProgressDialog();
        var result = await OnRetrieve();
        await Task.Delay(Delay);
        dlg.Close();

        this.SnackBar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);        
    }

    public virtual async Task Submit()
    {
        if (OnSubmit.xIsEmpty()) return;

        var dlg = await this.ShowProgressDialog();
        var result = await OnSubmit();
        await Task.Delay(Delay);
        dlg.Close();

        this.SnackBar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);
    }
}
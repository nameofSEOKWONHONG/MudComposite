﻿using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.DetailView;

public interface IMudDetailViewModel<TModel>: IMudViewModelBase
{
    TModel SelectedItem { get; set; }
    
    #region [event]

    Func<Task<Results>> OnSubmit { get; set; }
    Func<Task<Results>> OnRetrieve { get; set; }    

    #endregion
}
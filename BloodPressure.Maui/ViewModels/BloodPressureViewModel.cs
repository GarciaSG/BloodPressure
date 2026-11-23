using System.Collections.ObjectModel;
using BloodPressure.Maui.Helpers;
using BloodPressure.Maui.Services;
using BloodPressure.Maui.Views;
using BloodPressureApp.Core.DTOs;
using BloodPressureApp.Core.Validators;

namespace BloodPressure.Maui.ViewModels;

public class BloodPressureViewModel : BaseViewModel
{
    private readonly BloodPressureApiService _apiService;
    private readonly BloodPressureCreateValidator _createValidator;
    private readonly BloodPressureUpdateValidator _updateValidator;

    private BloodPressureDTO? _selectedMeasurement;
    private ActiveModal _activeModal;
    private string _systolicText = string.Empty;
    private string _diastolicText = string.Empty;
    private DateTime _recordedDate = DateTime.Today;
    private TimeSpan _recordedTime = DateTime.Now.TimeOfDay;
    private string _notes = string.Empty;
    private bool _isEditMode;
    private int? _editingId;
    private bool _hasListError;
    private string _listErrorMessage = string.Empty;

    public BloodPressureViewModel(
        BloodPressureApiService apiService,
        BloodPressureCreateValidator createValidator,
        BloodPressureUpdateValidator updateValidator)
    {
        _apiService = apiService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;

        BloodPressures = new ObservableCollection<BloodPressureDTO>();
        ValidationErrors = new ObservableCollection<string>();

        LoadMeasurementsCommand = new Command(async () => await LoadMeasurementsAsync());
        ShowDetailsCommand = new Command<BloodPressureDTO>(ShowDetails);
        OpenCreateFormCommand = new Command(async () => await OpenCreateFormAsync());
        OpenEditFormCommand = new Command(async () => await OpenEditFormAsync());
        OpenDeleteConfirmationCommand = new Command(OpenDeleteConfirmation);
        CloseModalCommand = new Command(CloseModal);
        ConfirmDeleteCommand = new Command(async () => await ConfirmDeleteAsync());
        SaveFormCommand = new Command(async () => await SaveFormAsync());
        CancelFormCommand = new Command(async () => await CancelFormAsync());

        _ = LoadMeasurementsAsync();
    }

    public ObservableCollection<BloodPressureDTO> BloodPressures { get; }

    public ObservableCollection<string> ValidationErrors { get; }

    public BloodPressureDTO? SelectedMeasurement
    {
        get => _selectedMeasurement;
        set
        {
            if (SetProperty(ref _selectedMeasurement, value))
            {
                OnPropertyChanged(nameof(SelectedMeasurementHasNotes));
            }
        }
    }

    public bool SelectedMeasurementHasNotes
        => !string.IsNullOrWhiteSpace(SelectedMeasurement?.Notes);

    public ActiveModal ActiveModal
    {
        get => _activeModal;
        set
        {
            if (SetProperty(ref _activeModal, value))
            {
                OnPropertyChanged(nameof(IsDetailsModalVisible));
                OnPropertyChanged(nameof(IsConfirmationModalVisible));
            }
        }
    }

    public bool IsDetailsModalVisible => ActiveModal == ActiveModal.Details;
    public bool IsConfirmationModalVisible => ActiveModal == ActiveModal.Confirmation;

    public bool HasValidationErrors => ValidationErrors.Count > 0;

    public string SystolicText
    {
        get => _systolicText;
        set => SetProperty(ref _systolicText, value);
    }

    public string DiastolicText
    {
        get => _diastolicText;
        set => SetProperty(ref _diastolicText, value);
    }

    public DateTime RecordedDate
    {
        get => _recordedDate;
        set => SetProperty(ref _recordedDate, value);
    }

    public TimeSpan RecordedTime
    {
        get => _recordedTime;
        set => SetProperty(ref _recordedTime, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            if (SetProperty(ref _isEditMode, value))
            {
                OnPropertyChanged(nameof(FormTitle));
                OnPropertyChanged(nameof(SaveButtonText));
            }
        }
    }

    public string FormTitle => IsEditMode ? "Editar medición" : "Nueva medición";
    public string SaveButtonText => IsEditMode ? "Actualizar" : "Guardar";

    public int? EditingId
    {
        get => _editingId;
        set => SetProperty(ref _editingId, value);
    }

    public bool HasListError
    {
        get => _hasListError;
        set
        {
            if (SetProperty(ref _hasListError, value))
            {
                OnPropertyChanged(nameof(IsListEmpty));
            }
        }
    }

    public string ListErrorMessage
    {
        get => _listErrorMessage;
        set => SetProperty(ref _listErrorMessage, value);
    }

    public bool IsListEmpty
        => !IsBusy && !HasListError && BloodPressures.Count == 0;

    public Command LoadMeasurementsCommand { get; }
    public Command<BloodPressureDTO> ShowDetailsCommand { get; }
    public Command OpenCreateFormCommand { get; }
    public Command OpenEditFormCommand { get; }
    public Command OpenDeleteConfirmationCommand { get; }
    public Command CloseModalCommand { get; }
    public Command ConfirmDeleteCommand { get; }
    public Command SaveFormCommand { get; }
    public Command CancelFormCommand { get; }

    private async Task LoadMeasurementsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        HasListError = false;
        ListErrorMessage = string.Empty;

        try
        {
            var data = await _apiService.GetAllAsync();

            BloodPressures.Clear();
            foreach (var item in data)
                BloodPressures.Add(item);

            OnPropertyChanged(nameof(IsListEmpty));
        }
        catch (BloodPressureApiException ex)
        {
            HasListError = true;
            ListErrorMessage = string.Join(" ", ex.Messages);
        }
        catch (Exception)
        {
            HasListError = true;
            ListErrorMessage = "Ocurrió un error inesperado al cargar las mediciones.";
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsListEmpty));
        }
    }

    private void ShowDetails(BloodPressureDTO dto)
    {
        if (dto is null) return;
        SelectedMeasurement = dto;
        ActiveModal = ActiveModal.Details;
    }

    private void OpenDeleteConfirmation()
    {
        if (SelectedMeasurement is null) return;
        ActiveModal = ActiveModal.Confirmation;
    }

    private void CloseModal()
    {
        ActiveModal = ActiveModal.None;
    }

    private async Task ConfirmDeleteAsync()
    {
        if (SelectedMeasurement is null) return;

        IsBusy = true;
        HasListError = false;
        ListErrorMessage = string.Empty;

        try
        {
            await _apiService.DeleteAsync(SelectedMeasurement.Id);

            ActiveModal = ActiveModal.None;
            SelectedMeasurement = null;
        }
        catch (BloodPressureApiException ex)
        {
            ActiveModal = ActiveModal.None;
            HasListError = true;
            ListErrorMessage = string.Join(" ", ex.Messages);
        }
        catch (Exception)
        {
            ActiveModal = ActiveModal.None;
            HasListError = true;
            ListErrorMessage = "Ocurrió un error inesperado al eliminar la medición.";
        }
        finally
        {
            IsBusy = false;
        }

        await LoadMeasurementsAsync();
    }

    private async Task OpenCreateFormAsync()
    {
        ClearValidationErrors();
        SystolicText = string.Empty;
        DiastolicText = string.Empty;
        RecordedDate = DateTime.Today;
        RecordedTime = DateTime.Now.TimeOfDay;
        Notes = string.Empty;
        EditingId = null;
        IsEditMode = false;
        ActiveModal = ActiveModal.None;

        try
        {
            await Shell.Current.GoToAsync(nameof(BloodPressureFormPage));
        }
        catch (Exception)
        {
            HasListError = true;
            ListErrorMessage = "No se pudo abrir el formulario. Intente nuevamente.";
        }
    }

    private async Task OpenEditFormAsync()
    {
        if (SelectedMeasurement is null) return;

        ClearValidationErrors();
        SystolicText = SelectedMeasurement.Systolic.ToString();
        DiastolicText = SelectedMeasurement.Diastolic.ToString();
        RecordedDate = SelectedMeasurement.RecordedAt.Date;
        RecordedTime = SelectedMeasurement.RecordedAt.TimeOfDay;
        Notes = SelectedMeasurement.Notes ?? string.Empty;
        EditingId = SelectedMeasurement.Id;
        IsEditMode = true;
        ActiveModal = ActiveModal.None;

        try
        {
            await Shell.Current.GoToAsync(nameof(BloodPressureFormPage));
        }
        catch (Exception)
        {
            HasListError = true;
            ListErrorMessage = "No se pudo abrir el formulario. Intente nuevamente.";
        }
    }

    private async Task CancelFormAsync()
    {
        ClearValidationErrors();

        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception)
        {
            AddValidationError("No se pudo regresar al listado. Intente nuevamente.");
        }
    }

    private async Task SaveFormAsync()
    {
        ClearValidationErrors();

        try
        {
            int? systolic = null;
            if (!string.IsNullOrWhiteSpace(SystolicText))
            {
                if (int.TryParse(SystolicText, out var s))
                    systolic = s;
                else
                {
                    AddValidationError("La presión sistólica debe ser un número entero.");
                    return;
                }
            }

            int? diastolic = null;
            if (!string.IsNullOrWhiteSpace(DiastolicText))
            {
                if (int.TryParse(DiastolicText, out var d))
                    diastolic = d;
                else
                {
                    AddValidationError("La presión diastólica debe ser un número entero.");
                    return;
                }
            }

            var recordedAt = RecordedDate.Date + RecordedTime;
            var notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes;

            if (IsEditMode)
            {
                var updateDto = new BloodPressureUpdateDTO
                {
                    Systolic = systolic,
                    Diastolic = diastolic,
                    RecordedAt = recordedAt,
                    Notes = notes
                };

                var validation = await _updateValidator.ValidateAsync(updateDto);
                if (!validation.IsValid)
                {
                    foreach (var error in validation.Errors)
                        AddValidationError(error.ErrorMessage);
                    return;
                }

                await SubmitUpdateAsync(updateDto);
            }
            else
            {
                var createDto = new BloodPressureCreateDTO
                {
                    Systolic = systolic,
                    Diastolic = diastolic,
                    RecordedAt = recordedAt,
                    Notes = notes
                };

                var validation = await _createValidator.ValidateAsync(createDto);
                if (!validation.IsValid)
                {
                    foreach (var error in validation.Errors)
                        AddValidationError(error.ErrorMessage);
                    return;
                }

                await SubmitCreateAsync(createDto);
            }
        }
        catch (Exception)
        {
            AddValidationError("Ocurrió un error inesperado al guardar la medición.");
        }
    }

    private async Task SubmitCreateAsync(BloodPressureCreateDTO dto)
    {
        IsBusy = true;
        try
        {
            await _apiService.CreateAsync(dto);
            await Shell.Current.GoToAsync("..");
        }
        catch (BloodPressureApiException ex)
        {
            foreach (var message in ex.Messages)
                AddValidationError(message);
        }
        catch (Exception)
        {
            AddValidationError("Ocurrió un error inesperado al guardar la medición.");
        }
        finally
        {
            IsBusy = false;
        }
        await LoadMeasurementsAsync();

    }

    private async Task SubmitUpdateAsync(BloodPressureUpdateDTO dto)
    {
        if (EditingId is null) return;

        IsBusy = true;
        try
        {
            await _apiService.UpdateAsync(EditingId.Value, dto);
            await Shell.Current.GoToAsync("..");
        }
        catch (BloodPressureApiException ex)
        {
            foreach (var message in ex.Messages)
                AddValidationError(message);
        }
        catch (Exception)
        {
            AddValidationError("Ocurrió un error inesperado al guardar la medición.");
        }
        finally
        {
            IsBusy = false;
        }
        await LoadMeasurementsAsync();

    }

    private void ClearValidationErrors()
    {
        ValidationErrors.Clear();
        OnPropertyChanged(nameof(HasValidationErrors));
    }

    private void AddValidationError(string message)
    {
        ValidationErrors.Add(message);
        OnPropertyChanged(nameof(HasValidationErrors));
    }
}
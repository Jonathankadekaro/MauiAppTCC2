using MauiAppTCC2.Models;
using MauiAppTCC2.Services;
using Microsoft.Maui.Controls;

namespace MauiAppTCC2
{
    public partial class MainPage : ContentPage
    {
        private readonly INotificationService _notificationService;

        public MainPage(INotificationService notificationService)
        {
            InitializeComponent();
            _notificationService = notificationService;
        }

        private async void OnTestNotificationClicked(object sender, EventArgs e)
        {
            var vacinaFake = new VacinaPet
            {
                Id = 1,
                Nome = "Antirrábica",
                PetId = 1,
                DataValidade = DateTime.Now.AddDays(10)
            };

            await _notificationService.ScheduleVaccineNotification(vacinaFake);

            await DisplayAlert("Notificação Agendada ✅",
                "Foi agendada uma notificação para simular a vacina 'Antirrábica'.",
                "OK");
        }
    }
}
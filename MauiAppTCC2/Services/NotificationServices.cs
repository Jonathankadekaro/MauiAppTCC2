using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Plugin.LocalNotification;
using System.Diagnostics;

namespace MauiAppTCC2.Services
{
    public interface INotificationService
    {
        Task ScheduleVaccineNotification(VacinaPet vacina); // ✅ ATUALIZADO
        Task CheckUpcomingVaccines();
        Task CancelNotification(int notificationId);
    }

    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _database;

        public NotificationService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task ScheduleVaccineNotification(VacinaPet vacina) // ✅ ATUALIZADO
        {
            try
            {
                var pet = await _database.GetPetAsync(vacina.PetId);
                if (pet == null) return;

                var notificationDate = vacina.DataValidade.AddDays(-30);

                if (notificationDate > DateTime.Now)
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = vacina.Id,
                        Title = "💉 Lembrete de Vacina Pet",
                        Description = $"{pet.Nome} precisa da vacina {vacina.Nome} em 30 dias",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = notificationDate
                        },
                        CategoryType = NotificationCategoryType.Reminder
                    };

                    LocalNotificationCenter.Current.Show(request);

                    vacina.NotificacaoAgendada = true;
                    await _database.SaveVacinaAsync(vacina);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erro ao agendar notificação: {ex.Message}");
            }
        }

        public async Task CheckUpcomingVaccines()
        {
            try
            {
                var vacinas = await _database.GetAllVacinasAsync();

                foreach (var vacina in vacinas)
                {
                    if (vacina.DataValidade.AddDays(-30) <= DateTime.Now &&
                        vacina.DataValidade >= DateTime.Now &&
                        !vacina.NotificacaoAgendada)
                    {
                        await ScheduleVaccineNotification(vacina);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erro ao verificar vacinas: {ex.Message}");
            }
        }

        public Task CancelNotification(int notificationId)
        {
            LocalNotificationCenter.Current.Cancel(notificationId);
            return Task.CompletedTask;
        }
    }
}
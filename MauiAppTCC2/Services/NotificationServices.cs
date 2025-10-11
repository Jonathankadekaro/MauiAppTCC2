using MauiAppTCC2.Data;
using MauiAppTCC2.Models;
using Plugin.LocalNotification;
using System.Diagnostics;


namespace MauiAppTCC2.Services
{
    public interface INotificationService
    {
        Task ScheduleVaccineNotification(Vacina vacina);
        Task CheckUpcomingVaccines();
        Task CancelNotification(int notificationId);
    }

    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _database;

        public NotificationService()
        {
            _database = new DatabaseContext();
        }

        public async Task ScheduleVaccineNotification(Vacina vacina)
        {
            try
            {
                var pet = await _database.GetPetAsync(vacina.PetId);
                if (pet == null) return;

                // Notificação 30 dias antes
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
                        CategoryType = NotificationCategoryType.Reminder,
                        Android = new AndroidOptions
                        {
                            ChannelId = "vaccine_reminders"
                        }
                    };

                    await LocalNotificationCenter.Current.Show(request);
                    vacina.NotificacaoAgendada = true;
                    await _database.SaveVacinaAsync(vacina);
                }

                // Notificação 7 dias antes (segunda notificação)
                var notificationDate7Days = vacina.DataValidade.AddDays(-7);
                if (notificationDate7Days > DateTime.Now)
                {
                    var request7Days = new NotificationRequest
                    {
                        NotificationId = vacina.Id + 10000, // ID diferente
                        Title = "💉 Vacina Próxima do Vencimento",
                        Description = $"{pet.Nome} - {vacina.Nome} vence em 7 dias!",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = notificationDate7Days
                        }
                    };

                    await LocalNotificationCenter.Current.Show(request7Days);
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

        public async Task CancelNotification(int notificationId)
        {
            await LocalNotificationCenter.Current.Cancel(notificationId);
        }
    }
}
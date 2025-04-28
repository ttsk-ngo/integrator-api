using System.ComponentModel;
using System.Runtime.CompilerServices;
using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.WebInterface.DisplayModels;

namespace Integrator.Frontend.WebInterface.ViewModels;

public interface IComplaintsViewModel
{
    List<ComplaintDisplayModel> Complaints { get; }
    Task LoadComplaintsAsync();
    event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? propertyName = null);
}

public class ComplaintsViewModel : ViewModelBase, IComplaintsViewModel
{
    private List<ComplaintDisplayModel> _complaints = new();
    public List<ComplaintDisplayModel> Complaints
    {
        get => _complaints;
        private set => SetField(ref _complaints, value);
    }

    public async Task LoadComplaintsAsync()
    {
        _complaints = new List<ComplaintDisplayModel>()
        {
            new ComplaintDisplayModel()
            {
                Number = "1/2025",
                InvolvedUsers =
                {
                    new InvolvedUserDisplayModel()
                    {
                        Role = InvolvedUser.InvolvedUserRole.Accuser,
                        UserId = "1",
                        Username = "gagar"
                    },
                    new InvolvedUserDisplayModel()
                    {
                        Role = InvolvedUser.InvolvedUserRole.Accused,
                        UserId = "2",
                        Username = "stonka"
                    },
                    new InvolvedUserDisplayModel()
                    {
                        Role = InvolvedUser.InvolvedUserRole.Witness,
                        UserId = "3",
                        Username = "byk"
                    }
                },
                Status = Complaint.ComplaintStatus.Open,
                Context = Complaint.ComplaintContext.TrainDriver2,
                Description = "Używanie botów w symulatorze",
                CreatedAt = DateTime.Now.AddDays(-5)
            }
        };
    }
}

/*
 * Complains = new List<Complain>
            {
                new Complain {
                    Number = 1,
                    Sign = "CPL-001",
                    Nickname = "ShadowHunter42",
                    Context = "Symulator",
                    Victim = "BlazeFury",
                    Accused = "ShadowHunter42",
                    Witness = "ThunderBolt77",
                    Description = "Używanie botów w symulatorze",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    Status = "Open"
                },
                new Complain {
                    Number = 2,
                    Sign = "CPL-002",
                    Nickname = "NightRider99",
                    Context = "SWDR",
                    Victim = "NightRider99",
                    Accused = "ThunderBolt77",
                    Witness = "StealthNinja21",
                    Description = "Łamanie kodeksu jazdy SWDR",
                    CreatedDate = DateTime.Now.AddDays(-3),
                    Status = "In progress"
                },
                new Complain {
                    Number = 3,
                    Sign = "CPL-003",
                    Nickname = "BlazeFury",
                    Context = "chat",
                    Victim = "StealthNinja21",
                    Accused = "BlazeFury",
                    Witness = "ShadowHunter42",
                    Description = "Używanie wulgaryzmów na czacie",
                    CreatedDate = DateTime.Now.AddDays(-1),
                    Status = "Closed"
                },
                new Complain {
                    Number = 4,
                    Sign = "CPL-004",
                    Nickname = "ThunderBolt77",
                    Context = "forum",
                    Victim = "ThunderBolt77",
                    Accused = "NightRider99",
                    Witness = "",
                    Description = "Publikowanie niewłaściwych treści",
                    CreatedDate = DateTime.Now,
                    Status = "Open"
                },
                new Complain {
                    Number = 5,
                    Sign = "CPL-005",
                    Nickname = "StealthNinja21",
                    Context = "inne",
                    Victim = "BlazeFury",
                    Accused = "StealthNinja21",
                    Witness = "NightRider99",
                    Description = "Oszustwo",
                    CreatedDate = DateTime.Now.AddHours(-12),
                    Status = "In progress"
                }
            };
 */
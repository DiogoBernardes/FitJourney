using BusinessLogic.Models.Sports;

namespace FitJourney_BackEnd.Interface;

public interface ISportsRepository
{
    Task<List<ListSportsModel>> GetSports();
    Task<ListSportsModel> CreateSport(CreateSportModel sport);
    Task UpdateSport(UpdateSportModel sport);
    Task DeleteSport(int name);
    Task<ListSportsModel> GetSportByName(string name);
}
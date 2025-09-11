namespace Application.Services.Interfaces;

public interface IListingPathProvider
{
    string GetFilteredPath();
    string GetNonFilteredPath();

    string BuildFilteredPath(params object[]? specs);
    string BuildNonFilteredPath(params object[]? specs);
    string BuildDaysSinceCreatedOnly(object specs);
}

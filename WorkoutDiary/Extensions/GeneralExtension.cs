namespace WorkoutDiary.Extensions;

public static class GeneralExtension
{
    public static int GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User == null)
        {
            return default(int);
        }

        return int.TryParse(httpContext.User.Claims.Single(x => x.Type == "uid").Value, out int userId) ? userId : default(int);
    }
    public static string GetUserRole(this HttpContext httpContext)
    {
        if (httpContext.User == null)
        {
            return string.Empty;
        }

        return httpContext.User.Claims.Single(x => x.Type == "role").Value;
    }
}
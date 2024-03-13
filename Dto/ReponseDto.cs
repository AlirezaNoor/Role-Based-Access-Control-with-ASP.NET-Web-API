namespace RoleBasedAuthSample.Dto;

public class ReponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Data{ get; set; }
}

namespace DO;

//enum{ Beginner, Advanced Beginner, Intermediate,Advanced,Expert};
public record Worker
(
    int Id,
    DO.WorkerExperience Level,
    string? Email=null,
    double? Cost=null,
    string? Name=null
 )

{
    public Worker() : this(0, 0, "", 0.0, "") { }
}


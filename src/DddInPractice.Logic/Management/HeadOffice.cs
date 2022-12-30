using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;

namespace DddInPractice.Logic.Management;

public class HeadOffice : AggregateRoot
{
    /// <summary>
    /// All the payments made from users' cards (aka they deposited their money in the "bank").
    /// </summary>
    public virtual decimal Balance { get; set; }

    /// <summary>
    /// Money transferred from the cash machines.
    /// </summary>
    public virtual Money Cash { get; set; }
}
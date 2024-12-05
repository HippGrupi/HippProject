namespace hippserver.Models.Domain
{
    public enum OrderStatus
    {
        Created,
        InProgress,
        Labeled,
        Packaged,
        ReadyForShipping,
        Shipped,
        Completed
    }
}

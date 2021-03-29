namespace CapWatchBackend.Domain.Entities {
  public class Store : IEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentCapacity { get; set; }
  }
}

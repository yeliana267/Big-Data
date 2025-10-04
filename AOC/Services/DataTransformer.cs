using AOC.Context;
using Microsoft.EntityFrameworkCore; // usa EF Core en vez de EF6 (System.Data.Entity es viejo)

public class DataTransformer
{
    private readonly AppDbContext _context;

    public DataTransformer(AppDbContext context)
    {
        _context = context;
    }

    public Dictionary<string, int> EnsureMasterTable<T>(
        DbSet<T> dbSet,
        IEnumerable<string> valuesToEnsure,
        Func<T, string> getName,
        Action<T, string> setName,
        Func<T, int> getId,
        Action<T, int> setId
    ) where T : class, new()
    {
        // 1️⃣ Traer lo que ya existe
        var existing = dbSet.ToList();
        var map = existing.ToDictionary(getName, getId);

        // 2️⃣ Insertar los que faltan
        int nextId = existing.Any() ? existing.Max(getId) + 1 : 1;

        foreach (var val in valuesToEnsure.Distinct())
        {
            if (!map.ContainsKey(val))
            {
                var item = new T();
                setName(item, val);
                setId(item, nextId);
                dbSet.Add(item);

                map[val] = nextId;
                nextId++;
            }
        }

        // 👇 Aquí estaba el error
        _context.SaveChanges();
        return map;
    }
}

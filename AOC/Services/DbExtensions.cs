using Microsoft.EntityFrameworkCore;

public static class DbExtensions
{
    public static void AddIfNotExists<TEntity, TKey>(
        this DbContext context,
        IEnumerable<TEntity> entities,
        Func<TEntity, TKey> keySelector
    ) where TEntity : class
    {
        var dbSet = context.Set<TEntity>();

        var existingKeys = dbSet.AsNoTracking().Select(keySelector).ToHashSet();
        var newEntities = entities.Where(e => !existingKeys.Contains(keySelector(e))).ToList();

        if (newEntities.Any())
        {
            dbSet.AddRange(newEntities);
            context.SaveChanges();
        }
    }
}

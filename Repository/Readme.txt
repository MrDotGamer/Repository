
#DbContext working example with dynamically created DbSet and working add-migration
#All needed entities must be inherited from BaseEntity class.
public class GenericContext : DbContext
    {
        public GenericContext(DbContextOptions<GenericContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var basetype = typeof(BaseEntity).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseEntity)));

            foreach (var type in basetype)
            {
                if (type.IsClass)
                {
                    var modelBuilderEntityMethod = modelBuilder.GetType().GetMethod("Entity", new Type[] { });
                    var entityMethod = modelBuilderEntityMethod.MakeGenericMethod(new Type[] { type });
                    entityMethod.Invoke(modelBuilder, null);
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
# On Startup config example
            services.AddScoped<DbContext, GenericContext>();
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));
            services.AddDbContext<GenericContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
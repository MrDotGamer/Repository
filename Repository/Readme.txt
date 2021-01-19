
#DbContext working example with dynamically created DbSet and working add-migration
#All needed entities must be inherited from BaseEntity named class.
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
    # ToDo Reikia issiaiskinti del veikimo su attributais ir BaseEntity(Ar turi prasmes naudoti abu).kai klase turi properties kita klase,duomenu bazeje sukuriama jai papildoma lentele.
    # prisijunkti prie jos gali tiesiog initializaves klase konstruktoriuje.
    # ir nesvarbu ar paveldi BaseEntities ir ar turi attributa jai vistiek sukuriamas dbset.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var basetype = typeof(BaseEntity).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseEntity)));

            foreach (var type in basetype)
            {
                if (type.IsClass && type.GetCustomAttributes(typeof(DataBaseAttribute),true).Length > 0)
                {
                    var modelBuilderEntityMethod = modelBuilder.GetType().GetMethod("Entity", new Type[] { });
                    var entityMethod = modelBuilderEntityMethod.MakeGenericMethod(new Type[] { type });
                    entityMethod.Invoke(modelBuilder, null);
                }
            }
            base.OnModelCreating(modelBuilder);
        }
# On Startup config example
            services.AddScoped<DbContext, GenericContext>();
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));
            services.AddDbContext<GenericContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
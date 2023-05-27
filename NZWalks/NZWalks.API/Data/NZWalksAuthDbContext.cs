using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext :IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }



            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var readerId = "748fb7b2-52a9-4f3e-93e3-dd07c8b0e9ee";
            var writerId = "13cb0557-f68b-4b75-aeb6-4d38a813748d";


            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id=readerId,
                    ConcurrencyStamp= readerId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole()
                {
                    Id=writerId,
                    ConcurrencyStamp= writerId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);


        }
    }

    }


using Domain.Entites;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Seeders
{
    internal sealed class Seeder : ISeeder
    {
        private readonly AppDbContext _context;

        public Seeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Database.CanConnectAsync())
            {
                if (!_context.Plans.Any())
                {
                    var features = GetFeatures();
                    await _context.Features.AddRangeAsync(features);
                    var plans = GetPlans(features);
                    await _context.Plans.AddRangeAsync(plans);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private List<Feature> GetFeatures()
        {
            var seedDate = DateTime.UtcNow;

            return new List<Feature>
            {
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الطلاب",
                    Description = "إدارة حتى 100 طالب في المنصة.",
                    Key = "student_limit_basic",
                    CreatedAt = seedDate,
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الدروس",
                    Description = "إنشاء ما يصل إلى 50 درسًا.",
                    Key = "lesson_limit_basic",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "الاختبارات والواجبات",
                    Description = "أنشئ اختبارات وواجبات بسيطة لتقييم الطلاب.",
                    Key = "quiz_assignment_basic",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "دعم فني أساسي",
                    Description = "دعم عبر البريد الإلكتروني خلال أيام العمل.",
                    Key = "support_basic",
                    CreatedAt = seedDate
                },

                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الطلاب",
                    Description = "إدارة حتى 500 طالب.",
                    Key = "student_limit_growth",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الدروس والدورات",
                    Description = "إنشاء حتى 200 درس و10 دورات متكاملة.",
                    Key = "lesson_limit_growth",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "الاختبارات الذكية",
                    Description = "توليد اختبارات تلقائيًا باستخدام الذكاء الاصطناعي.",
                    Key = "ai_quiz_generation_growth",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "التحليلات والتقارير",
                    Description = "احصل على تقارير مفصلة حول تقدم الطلاب وتفاعلهم.",
                    Key = "analytics_growth",
                    CreatedAt = seedDate
                },
                new Feature                 {
                    Id = Guid.NewGuid(),
                    Name = "دعم فني متقدم",
                    Description = "دعم سريع عبر البريد الإلكتروني والدردشة.",
                    Key = "support_growth",
                    CreatedAt = seedDate
                },

                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الطلاب ",
                    Description = "إدارة عدد 1000 من الطلاب.",
                    Key = "student_limit_pro",
                    CreatedAt = seedDate
                },
                new Feature 
                {
                    Id = Guid.NewGuid(),
                    Name = "عدد الدروس والدورات غير محدود",
                    Description = "أنشئ عددًا غير محدود من الدروس والدورات.",
                    Key = "lesson_limit_pro",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "جلسات بث مباشر",
                    Description = "قم ببث محاضرات مباشرة والتفاعل مع الطلاب في الوقت الفعلي.",
                    Key = "live_sessions_pro",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "موقع مخصص للأكاديمية",
                    Description = "أنشئ موقعك التعليمي الكامل بعلامتك التجارية الخاصة.",
                    Key = "custom_website_pro",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "ذكاء اصطناعي متقدم",
                    Description = "ميزات AI لإنشاء الدروس، الكورسات، والأسئلة تلقائيًا.",
                    Key = "ai_features_pro",
                    CreatedAt = seedDate
                },
                new Feature
                {
                    Id = Guid.NewGuid(),
                    Name = "دعم فني على مدار الساعة",
                    Description = "دعم متميز متوفر 24/7 عبر جميع القنوات.",
                    Key = "support_pro",
                    CreatedAt = seedDate
                }
            };
        }

        private List<Plan> GetPlans(List<Feature> features)
        {
            var baseDate = DateTime.UtcNow;

            var studentLimitFeatureBasic = features.First(f => f.Key == "student_limit_basic");
            var lessonLimitFeatureBasic = features.First(f => f.Key == "lesson_limit_basic");
            var quizAssignmentFeatureBasic = features.First(f => f.Key == "quiz_assignment_basic");
            var supportFeatureBasic = features.First(f => f.Key == "support_basic");

            var studentLimitFeatureGrowth = features.First(f => f.Key == "student_limit_growth");
            var lessonLimitFeatureGrowth = features.First(f => f.Key == "lesson_limit_growth");
            var aiQuizFeatureGrowth = features.First(f => f.Key == "ai_quiz_generation_growth");
            var analyticsFeatureGrowth = features.First(f => f.Key == "analytics_growth");
            var supportFeatureGrowth = features.First(f => f.Key == "support_growth");

            var studentLimitFeaturePro = features.First(f => f.Key == "student_limit_pro");
            var lessonLimitFeaturePro = features.First(f => f.Key == "lesson_limit_pro");
            var liveSessionsFeaturePro = features.First(f => f.Key == "live_sessions_pro");
            var customWebsiteFeaturePro = features.First(f => f.Key == "custom_website_pro");
            var aiFeaturesFeaturePro = features.First(f => f.Key == "ai_features_pro");
            var supportFeaturePro = features.First(f => f.Key == "support_pro");


            return new List<Plan>
            {
                new Plan
                {
                    Id = Guid.NewGuid(),
                    Name = "الخطة الأساسية",
                    Slug = "basic",
                    Description = "ابدأ رحلتك التعليمية بالأدوات الأساسية لإنشاء الدروس وإدارة الطلاب بسهولة.",
                    CreatedAt = baseDate,
                    PlanPricings = new List<PlanPricing>
                    {
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 1200m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Monthly,
                            DiscountPercent = 0,
                            CreatedAt = baseDate
                        },
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 13000m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Annually,
                            DiscountPercent = 10,
                            CreatedAt = baseDate
                        }
                    },
                    PlanFeatures = new List<PlanFeature>
                    {
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = studentLimitFeatureBasic.Id,
                            LimitValue = 100,
                            LimitUnit = "طالب"
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = lessonLimitFeatureBasic.Id,
                            LimitValue = 50,
                            LimitUnit = "درس"
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = quizAssignmentFeatureBasic.Id,
                            LimitValue = 1,
                            LimitUnit = "أساسي"
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = supportFeatureBasic.Id,
                            LimitValue = 1,
                            LimitUnit = "أساسي"
                        }
                    }
                },

                new Plan
                {
                    Id = Guid.NewGuid(),
                    Name = "خطة النمو",
                    Slug = "growth",
                    Description = "طور أكاديميتك مع مزيد من الإمكانيات والمرونة في إدارة الطلاب والمحتوى. تشمل جميع ميزات الخطة الأساسية.",
                    CreatedAt = baseDate,
                    PlanPricings = new List<PlanPricing>
                    {
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 2990m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Monthly,
                            DiscountPercent = 0,
                            CreatedAt = baseDate
                        },
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 29900m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Annually,
                            DiscountPercent = 15,
                            CreatedAt = baseDate
                        }
                    },
                    PlanFeatures = new List<PlanFeature>
                    {
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = studentLimitFeatureGrowth.Id,
                            LimitValue = 500,
                            LimitUnit = "طالب",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = lessonLimitFeatureGrowth.Id,
                            LimitValue = 200,
                            LimitUnit = "درس",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = aiQuizFeatureGrowth.Id,
                            LimitValue = 1,
                            LimitUnit = "نشط",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = analyticsFeatureGrowth.Id,
                            LimitValue = 1,
                            LimitUnit = "نشط",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = supportFeatureGrowth.Id,
                            LimitValue = 1,
                            LimitUnit = "متقدم",
                        }
                    }
                },

                new Plan
                {
                    Id = Guid.NewGuid(),
                    Name = "الخطة الاحترافية",
                    Slug = "pro",
                    Description = "احصل على أقصى قدر من المرونة والميزات المخصصة لإدارة أكاديميتك باحترافية. تشمل جميع ميزات خطة النمو والخطة الأساسية.",
                    CreatedAt = baseDate,
                    PlanPricings = new List<PlanPricing>
                    {
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 6990m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Monthly,
                            DiscountPercent = 0,
                            CreatedAt = baseDate
                        },
                        new PlanPricing
                        {
                            Id = Guid.NewGuid(),
                            Price = 69900m,
                            Currency = "EGP",
                            BillingCycle = BillingCycle.Annually,
                            DiscountPercent = 20,
                            CreatedAt = baseDate
                        }
                    },
                    PlanFeatures = new List<PlanFeature>
                    {
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = studentLimitFeaturePro.Id,
                            LimitValue = 1000,
                            LimitUnit = "طالب",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = lessonLimitFeaturePro.Id,
                            LimitValue = -1,
                            LimitUnit = "غير محدود",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = liveSessionsFeaturePro.Id,
                            LimitValue = 1,
                            LimitUnit = "نشط",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = customWebsiteFeaturePro.Id,
                            LimitValue = 1,
                            LimitUnit = "موقع",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = aiFeaturesFeaturePro.Id,
                            LimitValue = 1,
                            LimitUnit = "نشط",
                        },
                        new PlanFeature
                        {
                            Id = Guid.NewGuid(),
                            FeatureId = supportFeaturePro.Id,
                            LimitValue = 1,
                            LimitUnit = "متميز",
                        }
                    }
                }
            };
        }
    }
}
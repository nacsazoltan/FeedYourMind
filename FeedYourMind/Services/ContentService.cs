using OkosDobozWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace OkosDobozWeb.Services
{
    // This service holds all content and provides it in the correct language
    public class ContentService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public ContentService(IVideoRepository videoRepository, IExerciseRepository exerciseRepository)
        {
            _videoRepository = videoRepository;
            _exerciseRepository = exerciseRepository;
        }

        // --- TEXTS ---
        private List<TextItem> GetHuTexts()
        {
            return new List<TextItem>
            {
                new TextItem { Topic = "basic", Title = "Általános tudnivalók", Description = "A kiegyensúlyozott és vegyes táplálkozással, valamint az étkezések helyes időzésével és összeállításával biztosíthatjuk a szervezetünk immunvédelmének megfelelő működését. Ebben a fejezetben a szélsőségektől mentes, rendszeres, vegyes, változatos, energiában, tápanyagokban az életkori és egyéni igényeknek megfelelő étrend összeállításához találsz információkat." },
                new TextItem { Topic = "food", Title = "Élelmiszer-csoportok", Description = "A gabonafélék, zöldségek-gyümölcsök, tej, tejtermékek, húsok, halak, a tojás, a zsírok és olajok és cukor és édesítőszerek mind-mind fontos elemei a kiegyensúlyozott táplálkozásnak. Ezek összetevőiről, táplálkozásban betöltött szerepéről találsz ebben a fejezeteben hasznos ismereteket." },
                new TextItem { Topic = "health", Title = "Egészség, betegség", Description = "Szervezetünk alapvető működésének ismerete szintén elengedhetetlen ahhoz, hogy megfelelő ismeretink legyenek az egészséges táplálkozás kialakításához. Ennek érdekében többek között az egészséges testösszetételről, a táplálkozással összefüggő betegségekről, azélelmiszerek által kiváltott táplálékallergiák és -intoleranciák mechanizmusáról találsz információt ebben a fejezetben." },
                new TextItem { Topic = "diet", Title = "Tudatos táplálkozás", Description = "Az egészségtudatos táplálkozás a bevásárlással kezdődik, amihez elengedhetetlen a tájékozott címke olvasás, amely elősegíti a megfelelő élelmiszer választást. A hiteles információszerzés, a helyes tárolás, a pazarlás elkerülése részei a tudatos táplálkozásnak. Ehhez kapcsolódóan számos hasznos információt találtok ebben a fejezetben." }
            };
        }

        private List<TextItem> GetEnTexts()
        {
            return new List<TextItem>
            {
                new TextItem { Topic = "basic", Title = "General Information", Description = "With a balanced and varied diet, as well as proper meal timing and composition, we can ensure the proper functioning of our body’s immune defense system. In this chapter, you will find information on how to create a regular, balanced, and diverse diet—free from extremes—that meets age-specific and individual energy and nutrient requirements." },
                new TextItem { Topic = "food", Title = "Food Groups", Description = "Cereals, vegetables and fruits, milk and dairy products, meats, fish, eggs, fats and oils, as well as sugar and sweeteners are all important components of a balanced diet. In this chapter, you will find useful information about their components and their role in nutrition." },
                new TextItem { Topic = "health", Title = "Health, Sickness", Description = "An understanding of the body’s basic functions is also essential for developing appropriate knowledge about healthy nutrition. To support this, this chapter provides information on topics such as healthy body composition, nutrition-related diseases, and the mechanisms of food-induced allergies and intolerances." },
                new TextItem { Topic = "diet", Title = "Eating Habits", Description = "Health-conscious nutrition begins with grocery shopping, where informed label reading is essential for making the right food choices. Obtaining reliable information, proper storage, and avoiding food waste are all parts of conscious eating. In this chapter, you will find a wealth of useful information related to these topics." }
            };
        }

        private List<TextItem> GetCsTexts()
        {
            return new List<TextItem>
            {
                new TextItem { Topic = "basic", Title = "Obecné informace", Description = "Vyváženou a pestrou stravou, stejně jako správným načasováním a sestavením jídel, můžeme zajistit správné fungování imunitního systému našeho organismu. V této kapitole naleznete informace k sestavení pravidelného, pestrého a vyváženého jídelníčku bez extrémů, který odpovídá energetickým a nutričním potřebám podle věku a individuálních požadavků." },
                new TextItem { Topic = "food", Title = "Skupiny potravin", Description = "Cereálie, zelenina a ovoce, mléko a mléčné výrobky, maso, ryby, vejce, tuky a oleje, stejně jako cukr a sladidla jsou všechny důležité součásti vyvážené stravy. V této kapitole naleznete užitečné informace o jejich složení a roli ve výživě." },
                new TextItem { Topic = "health", Title = "Zdraví, nemoci", Description = "Porozumění základním funkcím těla je také nezbytné pro rozvoj vhodných znalostí o zdravé výživě. K tomu tato kapitola poskytuje informace o tématech, jako je zdravé složení těla, nemoci související s výživou a mechanismy potravinových alergií a intolerancí." },
                new TextItem { Topic = "diet", Title = "Stravovací návyky", Description = "Zdravě uvědomělá výživa začíná nákupem potravin, kde je informované čtení etiket nezbytné pro správný výběr potravin. Získávání spolehlivých informací, správné skladování a předcházení plýtvání potravinami jsou všechny součásti uvědomělého stravování. V této kapitole naleznete spoustu užitečných informací k těmto tématům." }
            };
        }
        
        // Add GetCsTexts() here...

        // --- VIDEOS ---
            private List<TextItem> GetHuSustainabilityTexts()
            {
                return new List<TextItem>
                {
                    new TextItem { Topic = "ecosystem", Title = "Födünk ökoszisztémája", Description = "Földünk egy sokszínű összefüggő rendszer, annak ökoszisztémája számos alrendszer összefüggő működéséből épül fel. Ennek a csodálatos rendszernek a részleteit, ember és természet kölcsönhatásait, a környezetvédelem alávetéseit ismerheted meg ebben a fejezetben. Továbbá választ kaphatsz olyan kérdésekre, mint mit is jelent a fenntarthatóság, vagy hogy mitől olyan különleges bolygó a Föld." },
                    new TextItem { Topic = "environment", Title = "Élelmiszer termelés", Description = "A folyton növekvő fogyasztás és az élelmiszer pazarlása az iparosodás óta többlet termelést igényel. Emellett a világ népessége folyamatosan növekszik, amit az élelmiszertermelésnek is le kell követnie. Ismerd meg, hogy ezek a folyamatok milyen hatással vannak környezetünkre, illetve, hogy melyen mezőgazdasági és egyéb jógyakorlatokkal lehet enyhíteni a Földünkre nehezedő nyomást." },
                    new TextItem { Topic = "food-consumption", Title = "Fenntartható fogyasztás", Description = "A fenntartható élelmiszerfogyasztás legfőbb döntéshozói a fogyasztók. Fontos, hogy hazai, de legalábbis közelről érkezett terméket vásároljunk, ezzel se növelve ökológiai lábnyomunkat. Az élelmiszerek csomagolása, a termékválasztás mind-mind hatással van környezetünkre.  Ebben a fejezetben a tudatos élelmiszerfogyasztás, alapanyagok élelmiszer választáshoz kapcsolódó ismereteidet gyarapíthatod." }
                };
            }

            private List<TextItem> GetEnSustainabilityTexts()
            {
                return new List<TextItem>
                {
                    new TextItem { Topic = "ecosystem", Title = "The Ecosystem of Our Planet", Description = "Our planet is a diverse and interconnected system; its ecosystem is built upon the interrelated functioning of numerous subsystems. In this chapter, you can explore the details of this remarkable system, the interactions between humans and nature, as well as the fundamental principles of environmental protection. You will also find answers to questions such as what sustainability truly means and what makes Earth such a unique planet." },
                    new TextItem { Topic = "environment", Title = "Food Production", Description = "Ever-increasing consumption and food waste have required surplus production since the Industrial Revolution. In addition, the world’s population continues to grow, which food production must also keep pace with. In this chapter, you will learn about the impact of these processes on our environment, as well as the agricultural and other best practices that can help reduce the pressure placed on our planet." },
                    new TextItem { Topic = "food-consumption", Title = "Sustainable Consumption", Description = "Consumers are the primary decision-makers when it comes to sustainable food consumption. It is important to purchase domestic products—or at least those sourced locally—so as not to increase our ecological footprint. Food packaging and product choices all have an impact on our environment. In this chapter, you can expand your knowledge of conscious food consumption and learn more about selecting ingredients and food products responsibly." }
                };
            }

            private List<TextItem> GetCsSustainabilityTexts()
            {
                return new List<TextItem>
                {
                    new TextItem { Topic = "ecosystem", Title = "Ekosystém naší planety", Description = "Naše planeta je rozmanitý a propojený systém; její ekosystém je postaven na vzájemně propojeném fungování mnoha podsystémů. V této kapitole můžete prozkoumat podrobnosti tohoto pozoruhodného systému, interakce mezi lidmi a přírodou, stejně jako základní principy ochrany životního prostředí. Také zde najdete odpovědi na otázky, co skutečně znamená udržitelnost a co činí Zemi tak jedinečnou planetou." },
                    new TextItem { Topic = "environment", Title = "Výroba potravin", Description = "Stále rostoucí spotřeba a plýtvání potravinami vyžadovaly nadbytečnou produkci od průmyslové revoluce. Kromě toho populace světa stále roste, což musí výroba potravin také sledovat. V této kapitole se dozvíte o dopadu těchto procesů na naše životní prostředí, stejně jako o zemědělských a dalších osvědčených postupech, které mohou pomoci snížit tlak na naši planetu." },
                    new TextItem { Topic = "food-consumption", Title = "Udržitelná spotřeba", Description = "Spotřebitelé jsou hlavními rozhodovateli, pokud jde o udržitelnou spotřebu potravin. Je důležité nakupovat domácí produkty – nebo alespoň ty, které pocházejí z místních zdrojů – aby se nezvyšovala naše ekologická stopa. Balení potravin a výběr produktů mají všechny dopad na naše životní prostředí. V této kapitole můžete rozšířit své znalosti o vědomé spotřebě potravin a dozvědět se více o výběru ingrediencí a potravinových produktů odpovědně." }
                };
            }

        // --- VIDEOS ---
        // Videos are now retrieved from the database table 'videolist' via IVideoRepository.

        // --- EXERCISES ---
        private List<ExerciseItem> GetHuExercises()
        {
            return new List<ExerciseItem>
            {
                new ExerciseItem { Topic = "basic", Name = "Nem csak a fehérjétől nő az izom", ExerciseId = "2638" },
                new ExerciseItem { Topic = "basic", Name = "Nem minden zsír olaj, de minden olaj zsír.", ExerciseId = "2639" },
                new ExerciseItem { Topic = "food", Name = "Játék: Ételpiramis", ExerciseId = "/exercises/food.html" },
                new ExerciseItem { Topic = "health", Name = "Feladat: Vitaminok", ExerciseId = "/exercises/health.html" },
                new ExerciseItem { Topic = "diet", Name = "Kvíz: Szokások", ExerciseId = "/exercises/diet.html" }
            };
        }
        
        // Add GetEnExercises() and GetCsExercises() here (they can return the same data if URLs are not language-specific)

        
                // --- SUSTAINABILITY EXERCISES ---
                private List<ExerciseItem> GetHuSustainabilityExercises()
                {
                    return new List<ExerciseItem>
                    {
                        new ExerciseItem { Topic = "environment", Name = "Játék: Környezetvédelem", ExerciseId = "/exercises/environment.html" },
                        new ExerciseItem { Topic = "ecosystem", Name = "Feladat: Ökoszisztéma", ExerciseId = "/exercises/ecosystem.html" },
                        new ExerciseItem { Topic = "food-consumption", Name = "Kvíz: Tudatos fogyasztás", ExerciseId = "/exercises/food_consumption.html" }
                    };
                }

        // --- PUBLIC GETTERS ---
        // These methods return the correct data based on the current language
        
        public List<TextItem> GetTexts(string culture, string topic)
        {
            List<TextItem> texts;
            switch (culture)
            {
                case "en": texts = GetEnTexts(); break;
                case "cs": texts = GetCsTexts(); break; // Placeholder
                default: texts = GetHuTexts(); break;
            }
            return texts.Where(t => t.Topic == topic).ToList();
        }

        public List<VideoItem> GetVideos(string culture, string topic)
        {
            return _videoRepository.GetVideosAsync(culture, topic).GetAwaiter().GetResult();
        }

        public List<ExerciseItem> GetExercises(string culture, string topic)
        {
            return _exerciseRepository.GetExercisesAsync(culture, topic).GetAwaiter().GetResult();
        }

        // --- SUSTAINABILITY PUBLIC GETTERS ---
        public List<TextItem> GetSustainabilityTexts(string culture, string category)
        {
            List<TextItem> texts;
            switch (culture)
            {
                case "en": texts = GetEnSustainabilityTexts(); break;
                case "cs": texts = GetCsSustainabilityTexts(); break; // Placeholder
                default: texts = GetHuSustainabilityTexts(); break;
            }
            return texts.Where(t => t.Topic == category).ToList();
        }

        public List<VideoItem> GetSustainabilityVideos(string culture, string category)
        {
            return _videoRepository.GetVideosAsync(culture, category).GetAwaiter().GetResult();
        }

        public List<ExerciseItem> GetSustainabilityExercises(string culture, string category)
        {
            return _exerciseRepository.GetExercisesAsync(culture, category).GetAwaiter().GetResult();
        }
    }
}

using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using System.Collections;

namespace toi444
{
    class Program
    {
        static Regex stopSymbolsStart = new Regex("^(еще |него |сказать |а |ж |нее |со |без |же |ней |совсем |более |жизнь |нельзя |так |больше |за |нет |такой |будет |зачем |ни |там |будто |здесь |нибудь |тебя |бы |и |никогда |тем |был |из |ним |теперь |была |из-за |них |то |были |или |ничего |тогда |было |им |но |того |быть |иногда |ну |тоже |в |их |о |только |вам |к |об |том |вас |кажется |один |тот |вдруг |как |он |три |ведь |какая |она |тут |во |какой |они |ты |вот |когда |опять |у |впрочем |конечно |от |уж |все |которого |перед |уже |всегда |которые |по |хорошо |всего |кто |под |хоть |всех |куда |после |чего |всю |ли |потом |человек |вы |лучше |потому |чем |г |между |почти |через |где |меня |при |что |говорил |мне |про |чтоб |да |много |раз |чтобы |даже |может |разве |чуть |два |можно |с |эти |для |мой |сам |этого |до |моя |свое |этой |другой |мы |свою |этом |его |на |себе |этот |ее |над |себя |эту |ей |надо |сегодня |я |ему |наконец |сейчас |если |нас |сказал |есть |не |сказала )", RegexOptions.IgnoreCase);
        static Regex figures = new Regex("(\\d)");
        static Regex punctuationSymbols = new Regex("(\\W)");
        static Regex spaces = new Regex("(  )");
        static Regex stopSymbolsMiddle = new Regex("( еще | него | сказать | а | ж | нее | со | без | же | ней | совсем | более | жизнь | нельзя | так | больше | за | нет | такой | будет | зачем | ни | там | будто | здесь | нибудь | тебя | бы | и | никогда | тем | был | из | ним | теперь | была | изза | них | то | были | или | ничего | тогда | было | им | но | того | быть | иногда | ну | тоже | в | их | о | только | вам | к | об | том | вас | кажется | один | тот | вдруг | как | он | три | ведь | какая | она | тут | во | какой | они | ты | вот | когда | опять | у | впрочем | конечно | от | уж | все | которого | перед | уже | всегда | которые | по | хорошо | всего | кто | под | хоть | всех | куда | после | чего | всю | ли | потом | человек | вы | лучше | потому | чем | г | между | почти | через | где | меня | при | что | говорил | мне | про | чтоб | да | много | раз | чтобы | даже | может | разве | чуть | два | можно | с | эти | для | мой | сам | этого | до | моя | свое | этой | другой | мы | свою | этом | его | на | себе | этот | ее | над | себя | эту | ей | надо | сегодня | я | ему | наконец | сейчас | если | нас | сказал | есть | не | сказала )", RegexOptions.IgnoreCase);
        static Regex PERFECTIVEGERUND = new Regex("(ав|авши|авшись|яв|явши|явшись|ив|ивши|ившись|ыв|ывши|ывшись)$");
        static Regex REFLEXIVE = new Regex("(ся|сь)$");
        static Regex NOUN = new Regex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");
        static Regex VERB = new Regex("(ила|ла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю|ала|ана|аете|айте|али|ай|ал|аем|ан|ало|ано|ает|ают|аны|ать|аешь|анно|яла|яна|яете|яйте|яли|яй|ял|яем|ян|яло|яно|яет|яют|яны|ять|яешь|янно)$");
        static Regex ADJECTIVAL = new Regex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею|ившее|ившие|ившые|ившое|ившими|ившыми|ившей|ивший|ившый|ившой|ившем|ившим|ившым|ившом|ившего|ившого|ившему|ившому|ивших|ившых|ившую|ившюю|ившая|ившяя|ившою|ившею|ывшее|ывшие|ывшые|ывшое|ывшими|ывшыми|ывшей|ывший|ывшый|ывшой|ывшем|ывшим|ывшым|ывшом|ывшего|ывшого|ывшему|ывшому|ывших|ывшых|ывшую|ывшюю|ывшая|ывшяя|ывшою|ывшею|ующее|ующие|ующые|ующое|ующими|ующыми|ующей|ующий|ующый|ующой|ующем|ующим|ующым|ующом|ующего|ующого|ующему|ующому|ующих|ующых|ующую|ующюю|ующая|ующяя|ующою|ующею|аемее|аемие|аемые|аемое|аемими|аемыми|аемей|аемий|аемый|аемой|аемем|аемим|аемым|аемом|аемего|аемого|аемему|аемому|аемих|аемых|аемую|аемюю|аемая|аемяя|аемою|аемею|аннее|анние|анные|анное|анними|анными|анней|анний|анный|анной|аннем|анним|анным|анном|аннего|анного|аннему|анному|анних|анных|анную|аннюю|анная|анняя|анною|аннею|авшее|авшие|авшые|авшое|авшими|авшыми|авшей|авший|авшый|авшой|авшем|авшим|авшым|авшом|авшего|авшого|авшему|авшому|авших|авшых|авшую|авшюю|авшая|авшяя|авшою|авшею|ающее|ающие|ающые|ающое|ающими|ающыми|ающей|ающий|ающый|ающой|ающем|ающим|ающым|ающом|ающего|ающого|ающему|ающому|ающих|ающых|ающую|ающюю|ающая|ающяя|ающою|ающею|ащее|ащие|ащые|ащое|ащими|ащыми|ащей|ащий|ащый|ащой|ащем|ащим|ащым|ащом|ащего|ащого|ащему|ащому|ащих|ащых|ащую|ащюю|ащая|ащяя|ащою|ащею|яемее|яемие|яемые|яемое|яемими|яемыми|яемей|яемий|яемый|яемой|яемем|яемим|яемым|яемом|яемего|яемого|яемему|яемому|яемих|яемых|яемую|яемюю|яемая|яемяя|яемою|яемею|яннее|янние|янные|янное|янними|янными|янней|янний|янный|янной|яннем|янним|янным|янном|яннего|янного|яннему|янному|янних|янных|янную|яннюю|янная|янняя|янною|яннею|явшее|явшие|явшые|явшое|явшими|явшыми|явшей|явший|явшый|явшой|явшем|явшим|явшым|явшом|явшего|явшого|явшему|явшому|явших|явшых|явшую|явшюю|явшая|явшяя|явшою|явшею|яющее|яющие|яющые|яющое|яющими|яющыми|яющей|яющий|яющый|яющой|яющем|яющим|яющым|яющом|яющего|яющого|яющему|яющому|яющих|яющых|яющую|яющюю|яющая|яющяя|яющою|яющею|ящее|ящие|ящые|ящое|ящими|ящыми|ящей|ящий|ящый|ящой|ящем|ящим|ящым|ящом|ящего|ящого|ящему|ящому|ящих|ящых|ящую|ящюю|ящая|ящяя|ящою|ящею)$");
        static Regex DERIVATIONAL = new Regex("(ост|ость)$");
        static Regex SUPERLATIVE = new Regex("(ейш|ейше)$");
        static Regex NN = new Regex("(нн)$");
        static Regex SOFTSIGN = new Regex("(ь)$");
        static Regex AND = new Regex("(и)$");
        static char[] vowels = { 'а', 'о', 'у', 'ы', 'э', 'я', 'е', 'ю', 'и', 'А', 'О', 'У', 'Ы', 'Э', 'Я', 'Е', 'Ю', 'И' };
        static char[] consonants = { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ь' };

        static void Input( Model1 db)
        {
            for (int i=1; i<3;i++ )
            {
                Console.Write("Имя файла документа:   ");
                string inputFileName = Console.ReadLine();
                FileInfo fileInf = new FileInfo(inputFileName);
                if (!fileInf.Exists)
                {
                    Console.WriteLine("Ошибка. Неверное имя!");
                    Console.WriteLine("************************************");
                }
                else
                {
                    db.Documents.Add(new Document { Id = i, Name = inputFileName });
                    db.SaveChanges();

                }
            }
        }

        static void RemoveSpaces(Model1 db)
        {
            foreach (Document d in db.Documents)
            {
                using (StreamReader headings = new StreamReader("D:\\" + d.Id.ToString() + (d.Id).ToString() + (d.Id).ToString() + ".txt"))
                {
                    string newName = "D:\\" + (d.Id).ToString() + (d.Id).ToString() + (d.Id).ToString() + (d.Id).ToString()+".txt";
                    using (StreamWriter newHeadings = new StreamWriter(newName))
                    {
                        string line;
                        while ((line = headings.ReadLine()) != null)
                        {
                            line = spaces.Replace(line, " ");
                            newHeadings.WriteLine(line);
                        }
                    }
                }
            }
        }

        static void RemovingStopSymbols(Model1 db)
        {
            foreach (Document d in db.Documents)
            {
                using (StreamReader headings = new StreamReader("D:\\" + d.Id.ToString() +".txt"))
                {
                    string newName = "D:\\" + (d.Id).ToString() + (d.Id).ToString() + ".txt";
                    using (StreamWriter newHeadings = new StreamWriter(newName))
                    {
                        string line;
                        while ((line = headings.ReadLine()) != null)
                        {
                            line = stopSymbolsStart.Replace(line, "");
                            line = figures.Replace(line, "");
                            line = punctuationSymbols.Replace(line, " ");
                            line = stopSymbolsMiddle.Replace(line, " ");
                            line = spaces.Replace(line, " ");
                            newHeadings.WriteLine(line);
                        }
                    }
                }
            }
        }

        static string RVFinding(string word)
        {
            int position = -1;
            bool check = false;
            for (int i = 0; i < word.Length; i++)
            {
                int j = 0;
                foreach (char ch in vowels)
                {
                    if (word[i] == vowels[j])
                    {
                        position = i;
                        check = true;
                        break;
                    }
                    j++;
                }
                if (check)
                    break;
            }
            if ((position == -1) || (position == word.Length - 1))
                return "";
            else
            {
                string rv = "";
                for (int i = position + 1; i < word.Length; i++)
                    rv += word[i];
                return rv;
            }
        }

        static string R1Finding(string word)
        {
            string r1 = "";
            bool check = false;
            int pos = -1;
            if (word.Length < 2)
                return "";
            for (int i = 0; i < word.Length - 1; i++)
            {
                int f = 0;
                foreach (char c in vowels)
                {
                    if (word[i] == vowels[f])
                    {
                        check = true;
                        break;
                    }
                    f++;
                }
                if (check)
                {
                    f = 0;
                    foreach (char c in consonants)
                    {
                        if (word[i + 1] == consonants[f])
                        {
                            pos = i + 1;
                            break;
                        }
                        f++;
                    }
                }
                if (pos != -1)
                    break;
            }
            if (pos == -1)
                return "";
            for (int i = pos + 1; i < word.Length; i++)
                r1 += word[i];
            return r1;
        }

        static void Step1(string rv, ref string word)
        {
            if (PERFECTIVEGERUND.IsMatch(rv))
            {
                word = PERFECTIVEGERUND.Replace(word, "");
            }
            else
            {
                word = REFLEXIVE.Replace(word, "");
                if (ADJECTIVAL.IsMatch(rv))
                {
                    word = ADJECTIVAL.Replace(word, "");
                }
                else
                {
                    if (VERB.IsMatch(rv))
                    {
                        word = VERB.Replace(word, "");
                    }
                    else
                    {
                        if (NOUN.IsMatch(rv))
                        {
                            word = NOUN.Replace(word, "");
                        }
                    }
                }
            }
        }

        static void Step2(string rv, ref string word)
        {
            word = AND.Replace(word, "");
        }

        static void Step3(ref string word, string r2)
        {
            if (DERIVATIONAL.IsMatch(r2))
            {
                word = DERIVATIONAL.Replace(word, "");
            }
        }

        static void Step4(string rv, ref string word)
        {
            if (NN.IsMatch(rv))
            {
                word = NN.Replace(word, "н");
            }
            else
            {
                if (SUPERLATIVE.IsMatch(rv))
                {
                    word = SUPERLATIVE.Replace(word, "");
                    if (NN.IsMatch(rv))
                    {
                        word = NN.Replace(word, "н");
                    }
                }
                else
                {
                    if (SOFTSIGN.IsMatch(rv))
                    {
                        word = SOFTSIGN.Replace(word, "");
                    }
                }
            }
        }

        static void Stemming(Model1 db)
        {
            foreach (Document d in db.Documents)
            {
                using (StreamReader newHeadings = new StreamReader("D:\\" + d.Id.ToString() + d.Id.ToString() + ".txt"))
                {
                    using (StreamWriter newHeadingsAfterStemming = new StreamWriter("D:\\" + d.Id.ToString() + d.Id.ToString() + d.Id.ToString() + ".txt"))
                    {
                        string line;
                        while ((line = newHeadings.ReadLine()) != null)
                        {
                            int i = 0;
                            string word = "";
                            string rv, r1, r2;
                            line += " ";
                            line = line.ToLower();
                            while (i < line.Length)
                            {
                                if (line[i] != ' ')
                                {
                                    word += line[i];
                                }
                                else
                                {
                                    rv = RVFinding(word);
                                    if (rv != "")
                                    {
                                        r1 = R1Finding(word);
                                        r2 = R1Finding(r1);
                                        Step1(rv, ref word);
                                        Step2(rv, ref word);
                                        Step3(ref word, r2);
                                        Step4(rv, ref word);
                                    }
                                    newHeadingsAfterStemming.Write(word + " ");
                                    word = "";
                                }
                                i++;
                            }
                            newHeadingsAfterStemming.WriteLine("");
                        }
                    }
                }
            }
        }

        static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static int WordsCount(string str)
        {
            int count = 0;
            for(int i=0; i<str.Length; i++)
            {
                if (str[i] == ' ')
                    count++;
            }
            return count;
        }

        static void Shingles(Model1 db)
        {
            int id_shingle = -1;
            ArrayList docs = new ArrayList();
            foreach (Document d in db.Documents)
            {
                docs.Add(d.Id);
            }
            for(int k=0; k<docs.Count; k++)
            {
                using (StreamReader newHeadings = new StreamReader("D:\\" + docs[k].ToString() + docs[k].ToString() + docs[k].ToString() + docs[k].ToString()+".txt"))
                {
                    int pos = 0;
                    string line;
                    while ((line = newHeadings.ReadLine()) != null)
                    {
                        int check = 0;
                        int f = 0;
                        string word = "";
                        line = line.ToLower();
                        if(WordsCount(line) >2)
                        {
                            while (f < line.Length)
                            {
                                if ((line[f] != ' '))
                                {
                                    word += line[f];
                                }
                                else
                                {
                                    check++;
                                    if (check == 1)
                                        pos = f;
                                    if (check == 3)
                                    {
                                        if (word != "")
                                        {
                                            string hash = getMd5Hash(word);
                                            bool checking = false;
                                            foreach(Shingles s in db.Shingles)
                                            {
                                                if((s.hash == hash) && (s.id_text == (int)docs[k]))
                                                {
                                                    checking = true;
                                                    break;
                                                }
                                            }
                                            if(!checking)
                                            {
                                                id_shingle++;

                                                db.Shingles.Add(new Shingles { Id = id_shingle, id_text = (int)docs[k], hash = hash });
                                                db.SaveChanges();
                                            }
                                            word = "";
                                        }
                                        check = 0;
                                        f = pos;
                                    }
                                    else
                                        word += line[f];
                                }
                                f++;
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
        }

        static void Counting(Model1 db)
        {
            int doc1 = 0;
            int doc2 = 0;
            ArrayList shingles2 = new ArrayList();
            foreach (Shingles s in db.Shingles)
            {
                if (s.id_text == 1)
                {
                    doc1++;
                }
                else
                {
                    doc2++;
                    shingles2.Add(s.hash);
                }
            }
            int sum = 0;
            foreach(Shingles s in db.Shingles)
            {
                if (s.id_text == 1)
                {

                    for(int i=0; i< doc2; i++)
                    {
                        string hsh = (string)shingles2[i];
                        if(hsh == s.hash)
                        {
                            sum++;
                        }
                    }
                }
            }
            double percent = (double)sum*2 / (double)(doc1 + doc2);
            percent *= 100;
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("Степень схожести текстов:   {0:0.0}%", percent);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            using (Model1 db = new Model1())
            {
                Input(db);
                RemovingStopSymbols(db);
                Stemming(db);
                RemoveSpaces(db);
                Shingles(db);
                Counting(db);
            }
        }
    }
}

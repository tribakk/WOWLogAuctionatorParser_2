using System.Text;
using System.Threading.Tasks;

namespace WOWLogAuctionatorParser.Core
{
    public enum PriorityTag
    {
        one = 1,
        two = 2,
        three = 3
    };
    public enum ProductTag
    {
        ptNotFound = 0,
        ptZvezdniyMoch = 1,
        ptMorskoyStebel = 2,
        ptPozeluyZimi = 3,
        ptPilzaSireni = 4,
        ptRechnoyGoroh = 5,
        ptUkusAkundi = 6,
        ptYakorTrava = 7,


        ptBoevoyZelieVinoslivosti = 8,
        ptBoevoyZelieIntelecta = 9,
        ptBoevoyZelieLovkosti = 10,
        ptBoevoyZelieSili = 11,

        ptNastoyBezdonihGlubin = 12,
        ptNastoyBeskraynogoGorizonta = 13,
        ptNastoySiliBriboya = 14,
        ptNastoyStremitelnihTeheniy = 15
    };
}

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAuthentication.DB
{
    public class Player
    {
        [JsonProperty("Player_key")]
        public int Player_key { get; set; }
        [JsonProperty("FIRSTNAME")]
        public string FIRSTNAME { get; set; }

        [JsonProperty("LASTNAME")]
        public string LASTNAME { get; set; }

        [JsonProperty("AGE")]
        public int AGE { get; set; }

        [JsonProperty("GP")]
        public int GP { get; set; }

        [JsonProperty("MINS")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal MINS { get; set; }

        [JsonProperty("PLUS_MINUS")]
        [Column(TypeName = "decimal(5,1)")]
        public decimal PLUS_MINUS { get; set; }

        [JsonProperty("AST")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal AST { get; set; }

        [JsonProperty("BLK")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal BLK { get; set; }

        [JsonProperty("BLKA")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal BLKA { get; set; }

        [JsonProperty("OREB")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal OREB { get; set; }

        [JsonProperty("DREB")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal DREB { get; set; }

        [JsonProperty("FG_PCT")]
        [Column(TypeName = "decimal(5,3)")]
        public decimal FG_PCT { get; set; }

        [JsonProperty("FG3_PCT")]
        [Column(TypeName = "decimal(5,3)")]
        public decimal FG3_PCT { get; set; }

        [JsonProperty("FG3A")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal FG3A { get; set; }

        [JsonProperty("FG3M")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal FG3M { get; set; }

        [JsonProperty("FGA")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal FGA { get; set; }

        [JsonProperty("FGM")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal FGM { get; set; }

        [JsonProperty("FT_PCT")]
        [Column(TypeName = "decimal(5,3)")]
        public decimal FT_PCT { get; set; }

        [JsonProperty("FTA")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal FTA { get; set; }

        [JsonProperty("FTM")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal FTM { get; set; }

        [JsonProperty("W")]
        public int W { get; set; }

        [JsonProperty("L")]
        public int L { get; set; }

        [JsonProperty("W_PCT")]
        [Column(TypeName = "decimal(5,3)")]
        public decimal W_PCT { get; set; }

        [JsonProperty("PF")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal PF { get; set; }

        [JsonProperty("PFD")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal PFD { get; set; }

        [JsonProperty("REB")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal REB { get; set; }

        [JsonProperty("TOV")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal TOV { get; set; }

        [JsonProperty("STL")]
        [Column(TypeName = "decimal(3,1)")]
        public decimal STL { get; set; }

        [JsonProperty("PTS")]
        [Column(TypeName = "decimal(4,1)")]
        public decimal PTS { get; set; }

        public Player()
        {

        }

        public Player(string fIRSTNAME, string lastname, int aGE, int gP, decimal mINS, decimal pLUS_MINUS, decimal aST, decimal bLK, decimal bLKA, decimal oREB, decimal dREB, decimal fG_PCT, decimal fG3_PCT, decimal fG3A, decimal fG3M, decimal fGA, decimal fGM, decimal fT_PCT, decimal fTA, decimal fTM, int w, int l, decimal w_PCT, decimal pF, decimal pFD, decimal rEB, decimal tOV, decimal sTL, decimal pTS)
        {
            this.FIRSTNAME = fIRSTNAME;
            this.LASTNAME = lastname;
            this.AGE = aGE;
            this.GP = gP;
            this.MINS = mINS;
            this.PLUS_MINUS = pLUS_MINUS;
            this.AST = aST;
            this.BLK = bLK;
            this.BLKA = bLKA;
            this.OREB = oREB;
            this.DREB = dREB;
            this.FG_PCT = fG_PCT;
            this.FG3_PCT = fG3_PCT;
            this.FG3A = fG3A;
            this.FG3M = fG3M;
            this.FGA = fGA;
            this.FGM = fGM;
            this.FT_PCT = fT_PCT;
            this.FTA = fTA;
            this.FTM = fTM;
            this.W = w;
            this.L = l;
            this.W_PCT = w_PCT;
            this.PF = pF;
            this.PFD = pFD;
            this.REB = rEB;
            this.TOV = tOV;
            this.STL = sTL;
            this.PTS = pTS;
        }


    }
}
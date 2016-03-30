using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcodeToolkit
{
    public static class AddidtionsList
    {
        public static IList<AdditionItem> GetAll()
        {
            List<AdditionItem> items = new List<AdditionItem>();
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_ZAGRANICZNY, Label = "zagraniczny" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_PACZKA, Label = "paczka" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_POLECONY, Label = "polecony" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_PRIORYTET, Label = "priorytet" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_ZAPOTW, Label = "za potwierdzeniem odbioru" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_ZAPOTW2, Label = "za potwierdzeniem odbioru x2" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_POBRANIEA, Label = "pobranie na adres" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_POBRANIEB, Label = "pobranie na konto" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_DUZYROZMIAR, Label = "gabaryt B" });
            items.Add(new AdditionItem() { FlagValue = Additions.DOD_WEWNETRZNY, Label = "poczta wewnętrzna" });
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_EXPRESS, Label = "przesyłka ekspresowa" });
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_NIESTANDARD, Label = "przesyłka niestandardowa" });
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_PACZKAZAPOBRANIEM, Label = "paczka za pobraniem" }); 
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_POTWIERDZONY, Label = "przesyłka ZPO została potwierdzona" });
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_ZAMKNIETY, Label = "przesyłka została już uwzględniona w raporcie zamknięcia" });    
            //items.Add(new AdditionItem() { FlagValue = Additions.DOD_ZWROT, Label = "przesyłka została zwrócona" });
            
            

            
            return items;
        }


   public static IList<AdditionItem> GetAll(int flags)
        {
            IList<AdditionItem> items = GetAll();

            foreach (AdditionItem item in items)
            {
                item.Checked = (flags & (int)item.FlagValue)>=(int)item.FlagValue;
            }

            return items;
        }

   public static string GetConcatenatedLabels(int flags)
   {
       StringBuilder sb=new StringBuilder();
       IList<AdditionItem> items = GetAll();
       bool isFirst = true;

       foreach (AdditionItem item in items)
       {
           if ((flags & (int)item.FlagValue) >= (int)item.FlagValue)
           {
               if (!isFirst)
                   sb.Append(", ");
               else
                   isFirst = false;

               sb.Append(item.Label);
           }
       }
       return sb.ToString();

   }
   }



    public class AdditionItem
    {
        public Additions FlagValue {get;set;}
        public string Label{get;set;}
        public bool Checked { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcodeToolkit
{
    public enum ReturnData
    {
        BARCODE_OK = 0, // w przypadku pomyślnego zakończenia
        BARCODE_EPARAM = -1, // w przypadku błędnego parametru (wskaźnik ma wartość NULL lub jedna z wartości jest mniejsza od zera)
        BARCODE_ESIZE = -2 // ilość danych do umieszczenia w kodzie jest za duża
    }

    public enum Additions
    {
        BRAK = 0, 
        DOD_NIESTANDARD = 1, // - przesyłka niestandardowa
        DOD_ZAPOTW = 2+16, // - przesyłka za potwierdzeniem odbioru
        DOD_PRIORYTET = 4, // - przesyłka priorytetowa
        DOD_EXPRESS = 8, // - przesyłka ekspresowa
        DOD_POLECONY = 16, //  - przesyłka polecona
        DOD_POTWIERDZONY = 32, // - przesyłka ZPO została potwierdzona 
        DOD_ZAMKNIETY = 64, // - przesyłka została już uwzględniona w raporcie zamknięcia 
        DOD_PACZKA = 128, // - przesyłka jest paczką bez pobrania
        DOD_ZAGRANICZNY = 256, // - przesyłka zagraniczna, ma znaczenie strefa
        DOD_PACZKAZAPOBRANIEM = 512, //- paczka za pobraniem
        DOD_ZWROT = 1024, // - przesyłka została zwrócona (powinno wykluczać się z DOD_POTWIERDZONY) 
        DOD_DUZYROZMIAR = 2048, // - przesyłka o dużym rozmiarze (gabaryt B)
        DOD_WEWNETRZNY = 4096, // - przesyłka wewnętrzna
        DOD_POBRANIEA = 8192+512, // - pobranie A (przekaz pocztowy)
        DOD_POBRANIEB = 16384+512, // - pobranie B (na konto)
        DOD_ZAPOTW2 = 32768+16 // za potwierdzeniem x2(podwojna zwrotka)
    }
}

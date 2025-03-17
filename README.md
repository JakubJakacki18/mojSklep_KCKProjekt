# mojSklep

**mojSklep** to aplikacja umożliwiająca zarządzanie sklepem, 
skierowana zarówno do sprzedawców, jak i kupujących. 
Projekt oferuje dwa interfejsy użytkownika: tekstowy(Terminal.GUI) 
oraz graficzny (WPF), co pozwala na elastyczne korzystanie 
z aplikacji w zależności od preferencji użytkownika.

## Funkcjonalności

### Dla kupujących:
- Przeglądanie dostępnych produktów.
- Dodawanie produktów do koszyka.
- Zarządzanie koszykiem (zmiana ilości, usuwanie produktów).
- Finalizacja zakupów z wyborem metody płatności (gotówka, karta, BLIK).
- Przeglądanie historii zakupów.

### Dla sprzedawców:
- Dodawanie nowych produktów do oferty.
- Edytowanie istniejących produktów (nazwa, opis, cena, ilość, lokalizacja w magazynie).
- Usuwanie produktów z oferty.
- Przeglądanie listy produktów.

## Technologie
- **.NET 8.0**: Platforma do budowy aplikacji.
- **WPF**: Graficzny interfejs użytkownika.
- **Terminal.GUI**: Biblioteka odpowiedzialna za interfejs tekstowego.
- **Entity Framework**: Obsługa bazy danych.
- **SQL Server**: Przechowywanie danych aplikacji.

## Struktura projektu
### InterfaceSelector
Prosta aplikacja konsolowa służaca za wybór interfejsu.
Otwiera w zależności od wyboru **ConsoleApp** lub **WpfApp**.
### Library
Klasa biblioteczna zawierająca logikę całej aplikacji. Podzielona jest na 
Kontrolery oraz Modele. To tutaj jest napisana główna pętla programu. Zawiera również abstrakcję definiującą jak wyglądają funkcje widokowe.
### ConsoleApp
Odpowiada za interfejs tekstowy aplikacji, została tutaj użyta biblioteka **Terminal.GUI**.
Dzięki użyciu tej biblioteki udało się w programie 
wprowadzić możliwość posługiwania się myszką.
Wszystko da się zrobić za pomocą myszki/klawiatury. Kolorystyka była wzorowana na taka jaką posiada aplikacja `Midnight Commander`.  


### WpfApp
Odpowiada za graficzny interfejs użytkownika aplikacji. Jest luźnym portem aplikacji konsolowej. 

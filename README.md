# Aplikacja do gromadzenia i udostępniania zdjęć działająca w API

Zamysłem aplikacji jest, aby użytkownik mógł tworzyć nowe albumy i przesyłał na nie zdjęcia. Aplikacja działa tylko w API i nie posiada interfejsu graficznego oraz została napisana w architekturze [Clean Architecture]([http://example.com](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)). w ASP.NET.

## Projekt obejmuje następujące wymagania:
- Dodanie zdjęcia z opisem (tag, autor, szczegóły, aparat, status: publiczny, prywatne, ograniczony dostęp, inne)
- Usuwanie własnych zdjęć
- Edycję opisów (tagi, szczegóły, aparat)
- Zmiana statusu zdjęcia (np. z prywatnego na publiczne)
- Przeszukiwanie zdjęć na podstawie praw użytkownika i filtrów (admin ma dostęp do wszystkich)
- Pobieranie zdjęć zgodnie z prawami (publiczne, własne, admin każde)
- Zarządzanie użytkownikami: rejestracja, weryfikacja, generowanie żetonów. 
- Generowanie miniatur wraz z linkami dla własnych zdjęć. 
- Tworzenie i edycja albumów (zmiana nazwy, statusu) 
- Dodawanie, usuwanie lub przenoszenie zdjęcia między albumami 
- Usuwanie albumu z zawartością 
- Każdy użytkownik może ocenić zdjęcie i dodać komentarz (ocena i komentarz dotyczy wyłącznie obcych zdjęć, nie można oceniać własnego zdjęcia)

## Baza danych

W celu dokonania migracji należy w pierwszej kolejności 

Schemat bazy danych:

## Po włączeniu aplikacji ukazuje się Swagger z funkcjonalnością aplikacji:
![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/d8d4dcac-10ce-496d-9568-9a12754b2861)

## Użytkownik 

Dodawanie nowego użytkownika

Dodawanie admina

Logowanie

Autoryzacja

## Album

Stworzenie nowego albumu

Edycja istniejącego albumu

Wyświetlenie wszystkich albumów danego użytkownika

Wyświetlenie konkretnego albumu danego użytkownika

Usuwanie albumu (wraz z zawartością)

## Zdjęcia / Posty (Publish)
Przesyłanie nowego zdjęcia

Edycja danego zdjęcia

Wyświetlenie wszystkich zdjęć danego użytkownika

Wyświetlenie jednego, konkretnego zdjęcia danego użytkownika

Zmiana miejsca przechowywania zdjęcia z jednego albumu do drugiego

Uusnięcie konkretnego zdjęcia

Usunięcie wszystkich zdjęć danego użytkownika

## Polubienia 

Dodanie polubienia pod zdjęciem

Usunięcie polubienia pod zdjęciem

## Komentarze

Dodawanie komentarza pod zdjęciem

Edycja komentarza

Wyświetlenie wszystkich komentarzy

Wyświetlenie wszystkich komentarzy dla danego użytkownika

Wyświetlenie wszystkich komentarzy dla danego zdjęcia

Wyświetlenie danego komentarza po jego ID

Usunięcie komentarza

## Tagi

Utworzenie taga

Edycja taga

Wyświetlenie wszystkich tagów

Wyświetlenie jednego taga

Usunięcie taga

## Testy jednostkowe

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/d82e7581-4e3f-48dc-9515-bfb1885d5310)

## Testy integracyjne


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

W celu wykonania migracji należy w pierwszej kolejności określić nazwę serwera w pliku appsettings.json oraz dokonać migrację poprzez komendę update-database w konsoli.

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/7dade6aa-f20c-4d60-9874-f0f903b12de1)

Schemat bazy danych:

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/c5001713-9576-4043-9077-c946c2b44815)

## Swagger

Po włączeniu aplikacji ukazuje się Swagger z funkcjonalnością aplikacji:

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/d8d4dcac-10ce-496d-9568-9a12754b2861)

## Użytkownik 

Dodawanie nowego użytkownika (Rejestracja)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/f7d4b7ed-aca6-4384-8d77-dacd9b10ab80)

Dodawanie admina

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/7b558237-0ddb-46c5-9fba-526a8963990f)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/e9bc2307-04d4-45f0-b4c0-524e3c53b0cd)

Logowanie

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/d0a1393b-69dc-416c-8a77-a0791184be38)

Po zalogowaniu należy skopiować wygenerowany token

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/8f592e0f-dd0e-4166-92cc-aa7a765c89a3)

A następnie użyć go przy autoryzacji

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/8bed00e3-0ae8-416f-a4ba-803a20342fcb)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/793f120a-ad6a-422f-8038-05faa31e2178)

_W dokumentacji w celu przedstawienia całej funkcjonalności zostało użyte konto administratora_

## Album

Stworzenie nowego albumu

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/241b2121-62fc-4093-8988-e2192f0b5fb2)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/5fb0f209-c579-4a40-9bd9-f10267f50ad6)

Edycja istniejącego albumu

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/ffed14e1-9d95-47f1-a716-85f52794e697)

Wyświetlenie wszystkich albumów danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/99c5863b-75dd-40b6-9909-1c52fe9dd0a6)
_Warto zauważyć, że wyświetlanie danych na stronie posiada funckję stronnicowania. Można wybrać numer strony oraz liczbę wyświetlonych elementów do wyświetlenia._

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/82b1c70f-c693-4b16-aa94-e1405e993e57)

Wyświetlenie konkretnego albumu danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/97c906cd-c712-4000-a47b-a326c6dd0fe6)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/369386d1-efca-4931-a8e9-7fcadc42ab7c)

Usuwanie albumu (wraz z zawartością)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/eec0d397-1e55-46ef-914e-e5a61701918a)

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

## FakeData

## Testy jednostkowe

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/eb1a9f1c-8402-4ec7-a213-097cffb7f730)

## Testy integracyjne

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/a63ca40f-cc29-4398-b155-44e2f5e753f9)

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

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/04bb08f0-c1cf-4e54-b341-653111b2e26e)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/0d70a997-e460-4b71-ab7e-28cb8d32b736)

Wyświetlenie wszystkich albumów danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/99c5863b-75dd-40b6-9909-1c52fe9dd0a6)
_Warto zauważyć, że wyświetlanie danych na stronie posiada funckję stronnicowania. Można wybrać numer strony oraz liczbę wyświetlonych elementów do wyświetlenia._

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/82b1c70f-c693-4b16-aa94-e1405e993e57)

Wyświetlenie konkretnego albumu danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/97c906cd-c712-4000-a47b-a326c6dd0fe6)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/369386d1-efca-4931-a8e9-7fcadc42ab7c)

Usuwanie albumu (wraz z zawartością)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/eec0d397-1e55-46ef-914e-e5a61701918a)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/cec5f233-da5f-4e28-8f7d-dbb5a6bb46d1)

## Zdjęcia / Posty (Publish)

Przesyłanie nowego zdjęcia

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/3ea35d53-0801-4aea-b789-86e49109b275)

Edycja danego zdjęcia

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/0c4d6684-6f8e-43c1-a860-c7d6ab7e55ae)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/5b05c592-005e-40f4-8440-3405aeb05045)

Wyświetlenie wszystkich zdjęć danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/1a8742d2-a2b5-4203-9124-e51040977198)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/07523fc8-02bd-40ec-a1e7-6e28eacd273f)

Wyświetlenie jednego, konkretnego zdjęcia danego użytkownika

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/22622c56-52cc-4292-85a9-37b758ccf726)

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/cdca16e3-2811-4c7e-b1b7-4853dde24688)

Oprócz tego aplikacja oferuje funkcje zmiany miejsca przechowywania zdjęcia z jednego albumu do drugiego, usnięcia konkretnego zdjęcia oraz usunięcia wszystkich zdjęć danego użytkownika.

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/cf2acab9-dd71-48e0-839a-e5b03539174c)

## Polubienia 

Dodanie oraz usunięcie polubienia pod zdjęciem
![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/745d60c8-6797-46e8-8469-820e34239876)

Liczbę polubień można zauważyć przy wyświetlaniu danych o zdjęciu:
![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/b52d6603-a257-4bd3-9567-d3730d97428d)

## Komentarze

- Dodawanie komentarza pod zdjęciem
- Edycja komentarza
- Wyświetlenie wszystkich komentarzy
- Wyświetlenie wszystkich komentarzy dla danego użytkownika
- Wyświetlenie wszystkich komentarzy dla danego zdjęcia
- Wyświetlenie danego komentarza po jego ID
- Usunięcie komentarza

![image](https://github.com/TrzeZzZwY/API-project/assets/117681023/9170ec78-f31a-49c2-b658-056232a52712)

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

# Aplikacja desktopowa do zarządzania bazą klientów i zamówień

Aplikacja desktopowa napisana w C# z użyciem WinForms, która umożliwia zarządzanie bazą klientów dla konkretnej firmy zajmującej się sprzedażą stolarki drzwiowej i okiennej. Umożliwia dodawanie, edytowanie i usuwanie danych oraz przeszukiwanie bazy danych, która jest przechowywana na serwerze z bazą PostgreSQL.

Aplikacja jest podzielona na kilka modułów, które współpracują, tworząc spójny system do zarządzania firmą.
	• Pomiary – Moduł wspiera zarządzanie pomiarami, które są wykonywane przed zamówieniem usługi montażowej. Zawiera tabelę z pomiarami oraz możliwość dodawania nowych, filtrowania i oznaczania wykonanych pomiarów. Pracownik może na tej podstawie przejść do tworzenia zamówienia przypisanego do pomiaru.
	• Zamówienia – Centralny punkt do zarządzania wszystkimi zamówieniami. Pracownik może dodawać zamówienia, przypisywać do nich klientów i rachunki oraz łatwo przechodzić do innych modułów, jak Reklamacje czy Płatności. Zamówieniom przypisane są automatyczne numery i rachunki, które można szczegółowo edytować.
	• Montaże – Moduł zarządzania montażami zamówionych towarów. Umożliwia planowanie montażu, filtrowanie zamówień gotowych do montażu oraz przegląd zadań w kalendarzu. Aplikacja pozwala także generować harmonogram montaży w pliku arkusza kalkulacyjnego.
	• Płatności – Moduł ten gromadzi informacje finansowe. Dla każdego zamówienia przypisane są płatności, a dla klientów tworzone są rachunki. Pracownik może modyfikować płatności, śledzić zaliczki oraz scalać rachunki. Istnieje także opcja generowania listy dłużników w formacie xlsx.
	• Reklamacje – Umożliwia monitorowanie i obsługę zgłoszonych reklamacji. Zawiera szczegóły zgłoszeń, statusy oraz dane kontaktowe klientów. Pliki związane z reklamacją można łatwo dodawać poprzez pole do przeciągania, a pracownik może przejść do zamówienia, aby zobaczyć dodatkowe informacje.
Każdy moduł jest połączony z innymi, zapewniając płynny przepływ informacji i umożliwiając kompleksowe zarządzanie danymi klientów oraz zamówień.

# Technologie
	• Język programowania: C#
	• Framework: .NET WinForms
	• Baza danych: PostgreSQL

# Uwagi
Kod źródłowy zamieszczony w tym repozytorium został stworzony wyłącznie na potrzeby konkretnej firmy i udostępniony tutaj wyłącznie w celach referencyjnych. Kod nie jest przeznaczony do pobrania, używania ani adaptacji przez inne podmioty niż firma, dla której aplikacja została stworzona.
Dodatkowo, nie zamieszczam schematu bazy danych używanej przez aplikację. W związku z tym kod źródłowy nie jest kompletny i nie może być używany bezpośrednio przez osoby trzecie.

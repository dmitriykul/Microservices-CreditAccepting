# Microservices-CreditAccepting

В репозитории представлены два взаимодействующих между собой REST сервиса для реализации возможности приема заявки на кредит и получения статуса рассмотрения этой заявки.

# Application-acceptance-service - Сервис заявки на кредит

Принимает заявку на кредит, сохраняет её в базу данных. Обращается к другому сервису с данными этой заявки, изменяя у себя в базе статус одобрения заявки на основе результатов сервиса скоринга заявки.

**api/Application/create** - принимает заявку. Возвращает уникальный номер и Id принятой заявки.

**api/Application/status** - возвращает результат рассмотрения заявки с некоторыми деталями по заявке по её Id.

На рисунке ниже представлена упрощенная структура сервиса заявки на кредит

![Untitled Diagram-Page-3.drawio](https://github.com/dmitriykul/Microservices-CreditAccepting/blob/dev/readmeImages/Untitled%20Diagram-Page-3.drawio.png)



# Application-Scoring-Service-Сервис оценки возможности выдачи кредита

Сервис эмуляции работы скоринга - оценки возможности выдачи кредита. Возвращает True или False.

**api/ScoringApplication/evaluate** - возвращает решение по заявке.

На рисунке ниже представлена упрощенная структура сервиса оценки возможности выдачи кредита

![Untitled Diagram-Page-4.drawio](https://github.com/dmitriykul/Microservices-CreditAccepting/blob/dev/readmeImages/Untitled%20Diagram-Page-4.drawio.png)

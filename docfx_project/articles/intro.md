# Введение

Философие OpenGameFramework - это архитектура позволяющая легко переносить куски кода (модули) из 1 проекта в другой.
При добавлении новой возможности в игру, необходимо создать новый модуль. Для обеспечения низкой связности в фреймворке используется разрешение зависимостей модулей через свой Ioc (DI) контейнер, к которому нет доступа у модулей.

## Структура папок
Все модули OpenGameFramework и проекта в целом лежат в папке “Modules”, ее содержимое:
1. Assets/App.cs — точка входа в приложение, которая загружается всю игру, вся логика, все связи стартуют отсюда.
2. Modules/System — папка, в которой хранятся модули движка, обеспечивающие работу архитектуры. Если вам требуется менять архитектуру, то следует изучить именно эту папку, а при создании игр, ее трогать нет необходимости.
3. Modules/Domain — папка в которой содержатся модули игры или приложения или еще чего, что делается (это предметная область, то о чем будет данное приложение, в нашем контексте это игра).

В модулях могут храниться скрипты, префабы, анимации и прочее
Но предпочтительнее держать это все в папке Content. И добавлять их в мета-информацию модуля (`Правой кнопкой по папке или файлам -> OpenGameFramework -> Add files to Module`).
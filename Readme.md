# Flex Task Manager Bot
Flex Task Manager Bot - это Telegram-бот, разработанный для управления задачами. Он позволяет вам легко создавать, управлять и отслеживать свои задачи, а также организовывать их по категориям. Бот предоставляет удобный интерфейс для оптимизации вашего рабочего процесса.
## Особенности

- Интеграция с Telegram: Ваши задачи всегда под рукой в мессенджере Telegram.
- Распознавание дат и времени: Бот может распознавать даты и время напоминания в тексте задач.
- Гибкое управление задачами: Вы можете создавать, редактировать, удалять задачи, устанавливать напоминания и категории.

## Установка и развертывание

Следуйте этим шагам, чтобы развернуть Flex Task Manager Bot на вашем сервере:

1. Создайте своего бота в Telegram:

    - Откройте Telegram и найдите бота @BotFather.
    - Отправьте команду `/newbot`, чтобы создать нового бота.
    - Следуйте инструкциям от BotFather для выбора имени и получения токена для вашего бота.
2. Склонируйте репозиторий:
```shell
git clone https://github.com/DmitryBahtenkov/FlexTaskManagerBot.git
```
3. Перейдите в директорию проекта:
```shell
cd FlexTaskManagerBot
```
4. Добавьте информацию о боте в `appsettings.json`:
```json
   "BotOptions": {
       "AccessToken": "ВАШ_ТОКЕН_БОТА",
       "CallbackUrl": "https://{ВАШ_ДОМЕН}/api/v1/async-callback"
   }
```
5. Замените ВАШ_ТОКЕН_БОТА на токен, который вы получили от BotFather при создании бота. CallbackUrl - это URL-адрес, который будет использоваться для асинхронных обратных вызовов, таких как уведомления о задачах.
6. Создайте файл .env на основе .env.example и установите необходимые переменные окружения, включая настройки SMTP и другие параметры. Чтобы отключить подтверждение по email, укажите переменную `DISABLE_EMAIL_CONFIRMATION=1`
7. Запустите бота, используя Docker Compose:
```shell
docker-compose up -d 
```
8. Убедитесь, что бот будет доступен по https - телеграм не выполняет callback-запросы по http. Для этого можно воспользоваться сертификатами Let's Encrypt и настроить Nginx Reverse Proxy.

Установите Let's Encrypt и настройте сертификат для вашего домена. Затем настройте Nginx Reverse Proxy для проксирования запросов к вашему боту через HTTPS. Пример конфигурации Nginx:
```
server {
    listen 443 ssl;
    server_name example.com;

    ssl_certificate /etc/letsencrypt/live/example.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/example.com/privkey.pem;

    location / {
        proxy_pass http://localhost:5255;  # Замените на порт вашего бота
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
    
    # Другие настройки...

}
```
## Использование
Откройте Telegram и найдите созданного вами бота. Нажмите "Старт" (или введите команду "/start") и следуйте инструкциям.

## Обратная Связь
Мы всегда рады получить вашу обратную связь и предложения по улучшению проекта. Пишите Issues :)

Лицензия
Этот проект распространяется под лицензией MIT. Подробности смотрите в файле LICENSE.

Этот проект использует форк библиотеки HORS для распознавания даты и времени из свободного текста:
- https://github.com/DenisNP/Hors
- https://habr.com/ru/articles/471204/
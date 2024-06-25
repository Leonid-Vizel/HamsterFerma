# HamsterFerma - простая ферма хомяка
# Как пользоваться?
1) Зайти в телеграмм веб
   
2) Зайти в самого хомяка и прописать в консоль(F12) :
   
console.log("Use this address in your browser:", document.getElementsByTagName('iframe')[0].src = document.getElementsByTagName('iframe')[0].src.replace(/(tgWebAppPlatform=)[^&]+/, "$1android"))
3) Кликнуть по появившейся ссылке

![image](https://github.com/Leonid-Vizel/HamsterFerma/assets/90096356/e7de8585-30e3-4eb6-966a-9590829ddc44)
4) Зайти Приложение->Локальное хранилище-> Скопировать AuthToken

![image](https://github.com/Leonid-Vizel/HamsterFerma/assets/90096356/ad899c7a-c715-4f83-a31c-3abc7fd8a11d)
5) Вставить в файл appsetting.json Bearer -> Auth -> (ваш токен)
   
![image](https://github.com/Leonid-Vizel/HamsterFerma/assets/90096356/cf1e05a7-7606-437b-81b0-34a1385be5d1)

6) Запустить приложение
   P.S. Если вы будете запускать ферму в песочнице не забудьте про прокси, иначе велика вероятность блокировки!
#Как работает?
У хамстер комбат открытое api
https://api.hamsterkombat.io
Поэтому нет проблем чтобы стучать в него post или get запросами для различных действий.

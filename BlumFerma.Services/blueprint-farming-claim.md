https://game-domain.blum.codes/api/v1/farming/claim

curl 'https://game-domain.blum.codes/api/v1/farming/claim' \
  -X 'POST' \
  -H 'accept: application/json, text/plain, */*' \
  -H 'accept-language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3' \
  -H 'authorization: Bearer ' \
  -H 'content-length: 0' \
  -H 'origin: https://telegram.blum.codes' \
  -H 'priority: u=1, i' \
  -H 'sec-ch-ua: "Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'sec-fetch-dest: empty' \
  -H 'sec-fetch-mode: cors' \
  -H 'sec-fetch-site: same-site' \
  -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36'

{"availableBalance":"280.6","playPasses":6,"timestamp":1721466285045}

def start_farming(self):
        if 'farming' not in self.balance_data:
            logging.info("Запуск фарминга")
            result = self.post(URL_FARMING_START)
            logging.info(f"{result.status_code},  {result.text}")
            self.update_balance()
        elif self.balance_data["timestamp"] >= self.balance_data["farming"]["endTime"]:
            logging.info('Сгребаем нафармленное')
            result = self.post(URL_FARMING_CLAIM)
            logging.info(f"{result.status_code},  {result.text}")
        logging.info(f'Ожидание завершения фарминга {self.estimate_time} секунд')
        sleep(self.estimate_time)
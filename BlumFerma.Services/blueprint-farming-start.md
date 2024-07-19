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
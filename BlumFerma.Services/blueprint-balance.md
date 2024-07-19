https://game-domain.blum.codes/api/v1/user/balance

curl 'https://game-domain.blum.codes/api/v1/user/balance' \
  -H 'accept: application/json, text/plain, */*' \
  -H 'accept-language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3' \
  -H 'authorization: Bearer ' \
  -H 'origin: https://telegram.blum.codes' \
  -H 'priority: u=1, i' \
  -H 'sec-ch-ua: "Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'sec-fetch-dest: empty' \
  -H 'sec-fetch-mode: cors' \
  -H 'sec-fetch-site: same-site' \
  -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36'


{
    "availableBalance": "193",
    "playPasses": 4,
    "timestamp": 1721405603452,
    "farming": {
        "startTime": 1721405584196,
        "endTime": 1721434384196,
        "earningsRate": "0.002",
        "balance": "0.038"
    }
}
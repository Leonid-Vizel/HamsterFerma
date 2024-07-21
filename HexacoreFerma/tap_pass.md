curl 'https://ago-api.onrender.com/api/get-tap-passes' \
  -H 'accept: */*' \
  -H 'accept-language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3' \
  -H 'authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoxNDU0NjM2MTA2LCJ1c2VybmFtZSI6IlNoYXJwb0thcmFzYiIsInRpbWVzdGFtcCI6MTcyMTU1MjQ3Mi4zMDQ0MjA1fQ.O5ymx1RqR5ZbK5VcGsM_FFBaeyA-_BsOVm6KDxd1GUg' \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -H 'origin: https://ago-wallet.hexacore.io' \
  -H 'pragma: no-cache' \
  -H 'priority: u=1, i' \
  -H 'referer: https://ago-wallet.hexacore.io/' \
  -H 'sec-ch-ua: "Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'sec-fetch-dest: empty' \
  -H 'sec-fetch-mode: cors' \
  -H 'sec-fetch-site: cross-site' \
  -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36'

{
    "active_tap_pass": {
        "created_at": 1721552761.815671,
        "expires_at": 1721639161.815671,
        "name": "1_day"
    },
    "tap_passes": {
        "1_day": {
            "price_stars": 8,
            "price_ton": 0.02,
            "price_usd": 0.15,
            "user_cost": 150
        },
        "3_days": {
            "price_stars": 32,
            "price_ton": 0.09,
            "price_usd": 0.62,
            "user_cost": 625
        },
        "7_days": {
            "price_stars": 51,
            "price_ton": 0.14,
            "price_usd": 1.0,
            "user_cost": 1000
        }
    }
}
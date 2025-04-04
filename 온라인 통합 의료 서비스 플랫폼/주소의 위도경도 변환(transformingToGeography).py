import requests
import pandas as pd
import time

# 카카오 REST API 키 입력 (반드시 본인의 키를 사용)
KAKAO_API_KEY = "fbdd2e16006bc8e28fa78e3d9d369545"

# 주소를 위도, 경도로 변환하는 함수
def get_lat_lon_kakao(address):
    if not isinstance(address, str) or address.strip() == "":
        return None, None  # 빈 값이면 None 반환

    url = f"https://dapi.kakao.com/v2/local/search/address.json?query={address}"
    headers = {"Authorization": f"KakaoAK {KAKAO_API_KEY}"}

    try:
        response = requests.get(url, headers=headers)

        # HTTP 응답 코드 확인
        if response.status_code != 200:
            print(f"❌ API 요청 실패: {address} (코드 {response.status_code})")
            return None, None

        data = response.json()

        if data.get("documents"):
            lat = data["documents"][0]["y"]  # 위도
            lon = data["documents"][0]["x"]  # 경도
            return lat, lon
        else:
            print(f"❗ 주소 변환 실패: {address} (응답 데이터 없음)")
            return None, None
    except Exception as e:
        print(f"⚠️ 요청 오류 발생: {e}")
        return None, None


# 엑셀 파일 로드
file_path = "C:/Users/KB/Documents/카카오톡 받은 파일/11.xlsx"
df = pd.read_excel(file_path)

# H_Address 컬럼을 기반으로 위도, 경도 추가
df["Latitude"] = None
df["Longitude"] = None

for idx, row in df.iterrows():
    address = row["H_Address"]

    if pd.notna(address):  # NaN 값 체크
        lat, lon = get_lat_lon_kakao(address)
        df.at[idx, "Latitude"] = lat
        df.at[idx, "Longitude"] = lon
        time.sleep(0.3)  # API 요청 속도 조절

# 변환된 데이터를 새 엑셀 파일로 저장
output_file = "C:/Users/KB/Documents/카카오톡 받은 파일/updated_11.xlsx"
df.to_excel(output_file, index=False)

print(f"✅ 위도, 경도 변환 완료! 파일 저장: {output_file}")


package Encryption;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.Scanner;

public class may {
	
	public static void main(String[] args) throws Exception {
		
	
		Scanner st = new Scanner(System.in);
		String pw = st.nextLine();
		String salt = getSalt();
		String hashedPw = Hashing(pw.getBytes(), salt);
		System.out.println("입력받은 패스워드의 길이: "+pw.length());
		System.out.println("입력받은 pw를 getBytes로 바이트배열로의 변환 후 길이: "+pw.getBytes().length);
		System.out.println("Salt: "+salt);
		System.out.println("Salt의 길이: "+salt.length());
		System.out.println("해싱 함수로 변환한 후 최종 Pw: "+hashedPw+" 길이: "+hashedPw.length());
		
		
	}
	public static String Byte_to_String(byte[] password) {
		StringBuilder sb = new StringBuilder();
		
		for(int i = 0; i < password.length; i++) {
			sb.append(String.format("%02x", password[i]));
		}
		return sb.toString();
	}
	public static String getSalt() {
		SecureRandom rnd = new SecureRandom();
		byte[] message = new byte[16];
		rnd.nextBytes(message);
		return Byte_to_String(message);
	}
	public static String Hashing(byte[] password, String Salt) throws Exception{
		MessageDigest md = MessageDigest.getInstance("SHA-256");
		for(int i = 0; i < 10; i++) {
			int k = 1+i;
			String temp = Byte_to_String(password)+Salt;
			System.out.println(k+"번째 temp: "+temp);
			System.out.println(k+"번째 temp의 길이: "+temp.length());
			System.out.println();
			md.update(temp.getBytes());
			System.out.println(k+"번째 update 후의 temp: "+temp);
			System.out.println(k+"번째 update 후의 temp 길이: "+temp.length());
			System.out.println();
			password = md.digest();
			System.out.println(k+"번째 digest 실행 후의 패스워드(스트링 변환): "+Byte_to_String(password));
			System.out.println(k+"번째 digest 실행 후의 패스워드(스트링 변환) 길이: "+Byte_to_String(password).length());
			System.out.println();
		}
		String c = Byte_to_String(password);
		return c;
	}
		
}


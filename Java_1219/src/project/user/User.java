package project.user;

import java.security.MessageDigest;
import java.security.SecureRandom;

import project.db.DataBase;

public class User {
	
	private static final int SALT_SIZE = 16;
	private static DataBase db = new DataBase();
	
	public int check_UserId(String id) {
		
		if(db.check_Id(id)) {
			System.out.println("중복된 아이디가 있습니다. Id를 다시 설정해주세요");
			return 0;
		}
		
		else {
			System.out.println("중복된 아이디가 없습니다. 회원가입을 계속 진행해주세요");
			return 1;
		}
	}
	
	public int check_UserPw(String pw) {
		int length = pw.length();
		String[] specialCharacter = {"!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", 
				"?", "/", "<", ">", "~", "{", "}", "[", "]"};
		
		if(length >= 20 || length < 8) {
			System.out.println("패스워드는 8자리 이상 20자리 미만이어야 합니다. 다시 설정해주세요");
			return 0;
		}
		else if(!pw.contains(specialCharacter[0])){
			System.out.println("패스워드에는 특수문자가 포함되어야 합니다. 다시 설정해주세요");
			return 0;
		}
		else {
			for(int i = 0; i < specialCharacter.length; i++) {
				int YorN = 0;
				if(pw.contains(specialCharacter[i])) {
					YorN = 1;
				}
				return YorN;
		}

			
		}
		System.out.println("패스워드 조건에 부합합니다. 회원가입을 계속 진행합니다.");
		return 1;
	}
	
	public void set_User(String id, byte[] password) throws Exception{
		String SALT = getSALT();
		db.set_USER(id, Hashing(password, SALT), SALT);
	}
	
	public void get_User(String Id, byte[] password) throws Exception{
		String temp_salt = db.get_SALT(Id);
		String temp_pass = Hashing(password, temp_salt);
		if(db.check(Id, temp_pass)) {
			System.out.println("로그인 성공");
		}
		else {
			System.out.println("로그인 실패");
		}
	}
	
	private String Hashing(byte[] password, String Salt) throws Exception{
		MessageDigest md = MessageDigest.getInstance("SHA-256");
		for(int i = 0; i < 10000; i++) {
			String temp = Byte_to_String(password)+Salt;
			md.update(temp.getBytes());
			password = md.digest();
		}
		return Byte_to_String(password);
	}
	private String getSALT() throws Exception{
		SecureRandom rnd = new SecureRandom();
		byte[] temp = new byte[SALT_SIZE];
		rnd.nextBytes(temp);
		return Byte_to_String(temp);
	}
	
	private String Byte_to_String(byte[] temp) {
		StringBuilder sb = new StringBuilder();
		for(byte a: temp) {
			sb.append(String.format("%02x", a));
		}
		return sb.toString();
	}
	
	public void get_DataBase() {
		System.out.println(db);
	}
}

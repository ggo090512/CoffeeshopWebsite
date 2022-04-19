Create Database DTH_Coffee
USE DTH_Coffee
GO
CREATE TABLE KHACHHANG
(
	MaKH INT IDENTITY(1,1) ,
	HoTen NVARCHAR(50) NOT NULL,
	TK VARCHAR(50) UNIQUE,
	MK VARCHAR(50) NOT NULL,
	Email VARCHAR(100) UNIQUE,
	DiaChi NVARCHAR(50),
	DienThoai VARCHAR(20),
	NgaySinh DATETIME,
	CONSTRAINT PK_KHACHHANG PRIMARY KEY(MaKH)
)
GO
CREATE TABLE LOAICAFE
(
	MaLoai INT IDENTITY(1,1),
	TenLoaiCafe NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_LOAICAFE PRIMARY KEY(MaLoai)
)
GO
CREATE TABLE CAFE
(
	MaCafe INT IDENTITY(1,1),
	TenCafe NVARCHAR(50) NOT NULL,
	Gia INT CHECK (Gia>=0),
	MoTa NVARCHAR(MAX),
	Anh VARCHAR(50),
	NgayCapNhat DATETIME,
	MaLoai INT,
	CONSTRAINT PK_CAFE PRIMARY KEY(MaCafe),
	Constraint FK_LOAICAFE Foreign Key(MaLoai) References LOAICAFE(MaLoai)
)
GO
CREATE TABLE DONDATHANG
(
	MaDonHang INT IDENTITY(1,1),
	DaThanhToan BIT DEFAULT 0,
	TinhTrang BIT DEFAULT 0,
	NgayDat DATETIME,
	MaKH INT,
	CONSTRAINT PK_DONDATHANG PRIMARY KEY(MaDonHang),
	Constraint FK_KHACHHANG Foreign Key(MaKH) References KHACHHANG(MaKH)
)
GO
CREATE TABLE CHITIETDONHANG
(
	MaDonHang INT,
	MaCafe INT,
	Soluong INT CHECK(Soluong>0),
	Dongia INT Check(Dongia>=0),	
	CONSTRAINT PK_CTDatHang PRIMARY KEY(MaDonHang,Macafe),
	CONSTRAINT FK_DONDATHANG FOREIGN KEY (MaDonHang) REFERENCES DONDATHANG(MaDonHang),
	CONSTRAINT FK_CAFE FOREIGN KEY (MaCafe) REFERENCES CAFE(MaCafe)
)
GO
CREATE TABLE VAITRO
(
	MaVaiTro INT IDENTITY(1,1),
	TenVaiTro NVARCHAR(20),
	CONSTRAINT PK_VAITRO PRIMARY KEY(MaVaiTro),
)
GO
CREATE TABLE NHANVIEN
(
	MaND INT IDENTITY(1,1),
	MaVaiTro INT,
	TaiKoan NVARCHAR(30),
	MatKhau NVARCHAR(30),
	CONSTRAINT PK_NHANVIEN PRIMARY KEY(MaND),
	CONSTRAINT FK_VAITRO FOREIGN KEY (MaVaiTro) REFERENCES VAITRO(MaVaiTro)
)
GO
CREATE TABLE AD
(
	UserName VARCHAR(30),
	PassWord VARCHAR(30),
	Ten NVARCHAR(50),
	CONSTRAINT PK_ADMIN PRIMARY KEY(UserName)
)
GO

INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'CÀ PHÊ VIỆT NAM')
INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'CÀ PHÊ PHA MÁY')
INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'COLD BREW')
INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'CÀ PHÊ ĐÁ XAY')
INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'BÁNH VÀ SNACK')
INSERT dbo.LOAICAFE(TenLoaiCafe)
VALUES (N'CÀ PHÊ GÓI')	

INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Bạc Xỉu', 32000,
N'Theo chân những người gốc Hoa đến định cư tại Sài Gòn, 
Bạc sỉu là cách gọi tắt của "Bạc tẩy sỉu phé" trong tiếng Quảng Đông, 
chính là: Ly sữa trắng kèm một chút cà phê.',
'bac_siu.jpg','2/1/2021',1)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Cà Phê Đen', 32000,
N'Một tách cà phê đen thơm ngào ngạt, 
phảng phất mùi cacao là món quà tự thưởng tuyệt vời nhất cho những ai mê đắm tinh chất nguyên bản nhất của cà phê. 
Một tách cà phê trầm lắng, thi vị giữa dòng đời vồn vã.',
'cf_da.jpg','2/1/2021',1)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Cà Phê Sữa', 32000,
N'Cà phê phin kết hợp cũng sữa đặc là một sáng tạo đầy tự hào của người Việt, 
được xem món uống thương hiệu của Việt Nam.',
'cfsd.jpg','2/1/2021',1)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Americano', 39000,
N'Americano được pha chế bằng cách thêm nước vào một hoặc hai shot Espresso để pha loãng độ đặc của cà phê, 
từ đó mang lại hương vị nhẹ nhàng, không gắt mạnh và vẫn thơm nồng nàn.',
'americano.jpg','2/1/2021',2)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Cà Phê Lúa Mạch Đá', 69000,
N'',
'merry-coffee_lua_mach_da.jpg','2/1/2021',2)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Caramel Macchiato', 55000,
N'Vị thơm béo của bọt sữa và sữa tươi, 
vị đắng thanh thoát của cà phê Espresso hảo hạng, 
và vị ngọt đậm của sốt caramel.',
'bac_siu.jpg','2/1/2021',1)
INSERT dbo.CAFE(TenCafe,Gia,MoTa,Anh,NgayCapNhat,MaLoai)
VALUES (N'Cappccino', 45000,
N'Cappuccino được gọi vui là thức uống "một-phần-ba" - 1/3 Espresso, 1/3 Sữa nóng, 1/3 Foam.',
'cappuccino.jpg','24/1/2021',2)
# Text Editor

Aplikasi editor teks modern dengan antarmuka menggunakan C# Windows Forms.

## Fitur

- **Tema Gelap & Terang**: Dua tema yang bisa dipilih, dengan sudut melengkung dan gaya halus
- **Toolbar**: Toolbar dengan tombol-tombol utama yang bisa di-hide/show (Ctrl+T)
- **Operasi Standar**: New, Open, Save, Cut, Copy, Paste, Undo, Redo
- **Navigasi**: Find, Go To Line, Toggle Word Wrap
- **Undo/Redo**: Mendukung hingga 1000 operasi dengan deduplikasi cerdas
- **Encoding File**: Dukungan native untuk UTF-8, UTF-16 (BOM dan Unicode), dan fallback ke default sistem
- **Context Menu (Klik Kanan)**: Undo, Cut, Copy, Paste, Delete, Select All — mirip Notepad
- **Drag & Drop**: Bisa drag teks dari/to editor
- **Status Bar**: Menampilkan posisi kursor, jumlah baris/char, encoding, dan shortcut hint
- **Settings**: Theme, Font, Toolbar visibility, Right-click context menu registration — semua tersimpan
- **Right-Click Context Menu**: Opsi "Open with Text Editor" di Explorer (via registry)
- **Persistent Settings**: Window size/position, theme, font, word wrap, toolbar visibility — otomatis tersimpan
- **File Association**: Bisa dijalankan dengan path file sebagai argumen command line

## Persyaratan Sistem

- .NET 10.0 Windows SDK
- Windows 10 atau lebih baru

## Membangun dan Menjalankan

### Visual Studio

1. Buka file solusi `text-editor.sln`
2. Atur project `text_editor` sebagai project startup
3. Build dan run (F5)

### .NET CLI

```bash
# Bangun project
dotnet build

# Jalankan
dotnet run

# Jalankan dengan file
dotnet run -- "path/to/file.txt"
```

## Shortcut Keyboard

| Fungsi | Shortcut |
|--------|----------|
| File Baru | Ctrl+N |
| Buka File | Ctrl+O |
| Simpan File | Ctrl+S |
| Undo | Ctrl+Z |
| Redo | Ctrl+Y |
| Potong | Ctrl+X |
| Salin | Ctrl+C |
| Tempel | Ctrl+V |
| Pilih Semua | Ctrl+A |
| Cari | Ctrl+F |
| Ke Baris | Ctrl+G |
| Toggle Toolbar | Ctrl+T |

## Dukungan Encoding File

Editor teks ini mendukung beberapa encoding file:

- **UTF-8** dengan BOM: Encoding pilihan untuk teks Unicode
- **UTF-16/Unicode**: Untuk file-file Windows lama
- **Default Sistem**: Fallback untuk file lama

## Settings

Pengaturan tersimpan di `%AppData%\TextEditor\settings.json` dan otomatis diload saat aplikasi dimulai:

- **Theme**: Dark / Light
- **Font**: Nama dan ukuran font editor
- **Toolbar**: Tampilkan/sembunyikan toolbar
- **Word Wrap**: Aktifkan/nonaktifkan word wrap
- **Window**: Posisi, ukuran, dan status maximized
- **Right-Click Menu**: Registrasi "Open with Text Editor" di context menu Windows Explorer

## Struktur Project

- `Program.cs`: Logika aplikasi utama (semua UI dan fitur)
- `icon.png`: Icon aplikasi
- `text-editor.csproj`: Konfigurasi project .NET

## Cara Menggunakan

### Operasi Dasar

1. **Buat file baru**: Klik "New" atau tekan Ctrl+N
2. **Buka file existing**: Klik "Open" atau tekan Ctrl+O
3. **Simpan file**: Klik "Save" atau tekan Ctrl+S
4. **Cari teks**: Klik "Find" atau tekan Ctrl+F
5. **Naik ke baris**: Klik "Go To" atau tekan Ctrl+G
6. **Toggle word wrap**: Klik "Word Wrap" atau tekan Ctrl+W
7. **Sembunyikan toolbar**: Tekan Ctrl+T

### Context Menu (Klik Kanan)

Klik kanan pada area editor untuk akses cepat ke Undo, Cut, Copy, Paste, Delete, dan Select All.

### Encoding File

File secara otomatis dideteksi dan disimpan dalam format UTF-8. Editor menampilkan "UTF-8" di status bar.

### Performa

Sistem undo/redo secara cerdas mendeduplikasi operasi berurutan yang sama untuk menghemat memori dan meningkatkan performa.

## Pengembangan

Aplikasi ini awalnya dikembangkan dalam Win32 API C++ dan telah dikonversi ke C# Windows Forms. Styling tema gelap/terang diimplementasikan untuk memberikan pengalaman membaca yang modern dan nyaman.

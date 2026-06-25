# Text Editor

Aplikasi editor teks modern dengan antarmuka dengan bahasa C# (Windows Forms).

## Fitur

- **Tema Gelap Bersih**: UI gelap profesional dengan sudut melengkung dan gaya halus
- **Operasi Standar**: New, Open, Save, Cut, Copy, Paste
- **Navigasi**: Find, Go To Line, Toggle Word Wrap
- **Undo/Redo**: Mendukung hingga 1000 operasi dengan deduplikasi cerdas
- **Encoding File**: Dukungan native untuk UTF-8, UTF-16 (BOM dan Unicode), dan fallback ke default sistem
- **Shortcut Keyboard**: Navigasi keyboard lengkap termasuk kombinasi Ctrl+<key>
- **Status Bar**: Menampilkan posisi kursor, jumlah baris/char, dan info encoding
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
cs project-file

# Jalankan dari command line
cs project-file.exe "path/to/file.txt"
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
| Word Wrap | Ctrl+W |

## Dukungan Encoding File

Editor teks ini mendukung beberapa encoding file:

- **UTF-8** dengan BOM: Encoding pilihan untuk teks Unicode
- **UTF-16/Unicode**: Untuk file-file Windows lama
- **Default Sistem**: Fallback untuk file lama

## Struktur Project

- `Program.cs`: Logika aplikasi utama
- `text-editor.csproj`: Konfigurasi project .NET
- `bin/`: Output build (lihat .gitignore)

## Cara Menggunakan

### Operasi Dasar

1. **Buat file baru**: Klik "New" atau tekan Ctrl+N
2. **Buka file existing**: Klik "Open" atau tekan Ctrl+O
3. **Simpan file**: Klik "Save" atau tekan Ctrl+S
4. **Cari teks**: Klik "Find" atau tekan Ctrl+F
5. **Naik ke baris**: Klik "Go To" atau tekan Ctrl+G
6. **Toggle word wrap**: Klik "Word Wrap" atau tekan Ctrl+W

### Encoding File

File secara otomatis dideteksi dan disimpan dalam format UTF-8. Editor menampilkan "UTF-8" di status bar.

### Performa

Sistem undo/redo secara cerdas mendeduplikasi operasi berurutan yang sama untuk menghemat memori dan meningkatkan performa.

## Pengembangan

Aplikasi ini awalnya dikembangkan dalam Win32 API C++ dan telah dikonversi ke C# Windows Forms. Styling tema gelap diimplementasikan untuk memberikan pengalaman membaca yang modern dan nyaman.

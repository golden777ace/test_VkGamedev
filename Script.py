import os
import sys
import subprocess
import ctypes
from playwright.sync_api import sync_playwright

def isAdmin():
    try:
        return ctypes.windll.shell32.IsUserAnAdmin()
    except:
        return False

def findDirectory():
    possible_directories = [
        os.path.expandvars("%ProgramFiles(x86)%\\Steam\\steamapps\\common\\Goose Goose Duck"),
        os.path.expandvars("%ProgramFiles%\\Steam\\steamapps\\common\\Goose Goose Duck"),
        "D:\\SteamLibrary\\steamapps\\common\\Goose Goose Duck",
        "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Goose Goose Duck",
        "D:\\Program Files\\Steam\\steamapps\\common\\Goose Goose Duck"
    ]
    for directory in possible_directories:
        if os.path.exists(directory):
            return directory
    return None

def applySettings(file_path):
    subprocess.run(['regedit', '/s', file_path], check=True)

def launchGame(game_dir):
    game_executable = os.path.join(game_dir, "Goose Goose Duck.exe")
    try:
        subprocess.Popen([game_executable])
    except Exception:
        subprocess.Popen(['steam'])

def downloadFile(url, download_path):
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)
        context = browser.new_context(accept_downloads=True)
        page = context.new_page()
        def setDownload(download):
            download.save_as(download_path)
        page.on("download", setDownload)
        page.goto(url)
        page.wait_for_selector('//*[@id="uc-download-link"]').click()
        page.wait_for_timeout(10000)
        browser.close()

def main():
    if not isAdmin():
        ctypes.windll.shell32.ShellExecuteW(None, "runas", sys.executable, __file__, None, 1)
        return

    url = "https://drive.google.com/uc?export=download&id=1IGENwFzLm8bBEboISadYSNEdxbnjz1fH"
    game_dir = findDirectory()
    if game_dir is None:
        return

    dest_file = os.path.join(game_dir, "settings.reg")
    downloadFile(url, dest_file)

    if os.path.exists(dest_file):
        applySettings(dest_file)
        subprocess.Popen(f'explorer /select,"{dest_file}"')

    launchGame(game_dir)

if __name__ == "__main__":
    main()

from pathlib import Path

def print_folder_tree(start_path, prefix=''):
    """
    使用 pathlib 只输出文件夹树状结构
    """
    path = Path(start_path)
    
    # 打印当前文件夹
    if prefix == '':
        print(f"📁 {path.name}/")
    else:
        print(f"{prefix} 📁 {path.name}/")
    
    # 获取所有子目录
    try:
        directories = [item for item in path.iterdir() if item.is_dir()]
    except PermissionError:
        print(f"{prefix}   [权限拒绝]")
        return
    
    # 递归处理子目录
    for i, directory in enumerate(directories):
        is_last = (i == len(directories) - 1)
        connector = '└── ' if is_last else '├── '
        new_prefix = prefix + ('    ' if is_last else '│   ')
        print(f"{prefix}{connector}", end='')
        print_folder_tree(directory, new_prefix)

# 使用示例
path = input("输入文件夹：")
print_folder_tree(path)
input("按任意键退出...")
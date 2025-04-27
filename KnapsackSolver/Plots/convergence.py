import subprocess, sys, os
import pandas as pd
import matplotlib.pyplot as plt

exe_dir = os.path.abspath(os.path.join("..", "KnapsackSolver", "bin", "Debug", "net8.0"))
exe     = os.path.join(exe_dir, "KnapsackSolver.exe")

if not os.path.exists(exe):
    print("❌ C# exe nenalezeno:", exe)
    sys.exit(1)

print("▶ Spouštím C# solver…\n")
res = subprocess.run([exe], cwd=exe_dir, capture_output=True, text=True)

if res.returncode != 0:
    print("❌ C# skončil chybou:\n", res.stderr)
    sys.exit(res.returncode)

print(f"✔ C# běh dokončen, CSV vytvořena v: {exe_dir}\n")
for lbl, color in [("bf","black"), ("rs","steelblue"), ("sa","crimson")]:
    path = os.path.join(exe_dir, f"{lbl}.csv")
    if not os.path.exists(path):
        print(f"⚠ CSV chybí: {path}")
        continue
    df = pd.read_csv(path, names=["FES","BEST"])
    plt.plot(df.FES, df.BEST, label=lbl.upper(), color=color)

plt.xlabel("FES")
plt.ylabel("Best value")
plt.legend()
plt.tight_layout()

out_png = os.path.abspath("convergence.png")
plt.savefig(out_png)

print(f"✔ Graf uložen jako {out_png}\n")
plt.show()